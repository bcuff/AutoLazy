using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;
using FieldAttributes = Mono.Cecil.FieldAttributes;
using GenericParameterAttributes = Mono.Cecil.GenericParameterAttributes;
using MethodAttributes = Mono.Cecil.MethodAttributes;

namespace AutoLazy.Fody
{
    internal class KeyedLazyWeaver : LazyWeaver
    {
        TypeReference _dictionaryRef;
        MethodReference _dictionaryCtor;
        MethodReference _dictionaryGetOrAdd;
        FieldDefinition _dictionaryField;
        FieldReference _dictionaryFieldRef;

        MethodReference _funcCtor;

        public KeyedLazyWeaver(MethodDefinition method, VisitorContext context)
            : base(method, context)
        {
        }

        protected override bool Validate()
        {
            var valid = true;
            if (Method.Parameters.Count > 1)
            {
                Context.LogError("[Lazy] methods may have at most 1 parameter");
                valid = false;
            }
            return valid && base.Validate();
        }

        protected override void InitializeTypes()
        {
            base.InitializeTypes();
            var parameterType = Method.Parameters[0].ParameterType;
            var resultType = Method.ReturnType;
            
            // Func
            _funcCtor = Method.Module.ImportReference(new MethodReference(".ctor", Method.Module.TypeSystem.Void, Method.Module.ImportReference(typeof(Func<,>)).MakeGenericInstanceType(parameterType, resultType))
            {
                HasThis = true,
                Parameters =
                {
                    new ParameterDefinition(Method.Module.TypeSystem.Object),
                    new ParameterDefinition(Method.Module.TypeSystem.IntPtr)
                }
            });
            
            // ConcurrentDictionary
            _dictionaryRef = Method.Module.ImportReference(typeof(ConcurrentDictionary<,>)).MakeGenericInstanceType(parameterType, resultType);
            var parameterTypeGeneric = new GenericParameter(_dictionaryRef);
            var resultTypeGeneric = new GenericParameter(_dictionaryRef);
            _dictionaryRef.GenericParameters.Add(parameterTypeGeneric);
            _dictionaryRef.GenericParameters.Add(resultTypeGeneric);
            _dictionaryCtor = Method.Module.ImportReference(new MethodReference(".ctor", Method.Module.TypeSystem.Void, _dictionaryRef) { HasThis = true });
            _dictionaryGetOrAdd = Method.Module.ImportReference(new MethodReference("GetOrAdd", resultTypeGeneric, _dictionaryRef)
            {
                HasThis = true,
                Parameters =
                {
                    new ParameterDefinition(parameterTypeGeneric),
                    new ParameterDefinition(Method.Module.ImportReference(typeof(Func<,>)).MakeGenericInstanceType(parameterTypeGeneric, resultTypeGeneric))
                }
            });
        }

        protected override void CreateFields()
        {
            base.CreateFields();
            var fieldAttributes = FieldAttributes.Private;
            if (Method.IsStatic) fieldAttributes |= FieldAttributes.Static;

            _dictionaryField = new FieldDefinition($"{Method.Name}$LazyCache", fieldAttributes, _dictionaryRef);
            Method.DeclaringType.Fields.Add(_dictionaryField);
            if (Method.DeclaringType.HasGenericParameters)
            {
                var declaringType = Method.DeclaringType.MakeGenericInstanceType(
                    Method.DeclaringType.GenericParameters.Cast<TypeReference>().ToArray());
                _dictionaryFieldRef = new FieldReference(_dictionaryField.Name, _dictionaryField.FieldType, declaringType);
            }
            else
            {
                _dictionaryFieldRef = _dictionaryField;
            }
        }

        protected override void InitializeFields(MethodDefinition ctor)
        {
            var il = ctor.Body.GetILProcessor();
            var start = ctor.Body.Instructions.First();
            if (!Method.IsStatic) il.InsertBefore(start, il.Create(OpCodes.Ldarg_0));
            il.InsertBefore(start, il.Create(OpCodes.Newobj, _dictionaryCtor));
            il.InsertBefore(start, il.Create(Method.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, _dictionaryFieldRef));
            base.InitializeFields(ctor);
        }

        protected override void WriteInstructions()
        {
            base.WriteInstructions();
            
            var clonedMethod = CloneMethod(Method);
            Method.DeclaringType.Methods.Add(clonedMethod);
            ClearMethod(Method);

            MethodReference clonedMethodRef;
            if (Method.DeclaringType.HasGenericParameters)
            {
                var declaringType = Method.DeclaringType.MakeGenericInstanceType(Method.DeclaringType.GenericParameters.Cast<TypeReference>().ToArray());
                clonedMethodRef = FixMethodReference(declaringType, clonedMethod);
            }
            else
            {
                clonedMethodRef = clonedMethod;
            }
            
            var il = Method.Body.GetILProcessor();
            if (!Method.IsStatic) il.Emit(OpCodes.Ldarg_0);
            il.Emit(Method.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, _dictionaryFieldRef);
            il.Emit(Method.IsStatic ? OpCodes.Ldarg_0 : OpCodes.Ldarg_1); // load param
            il.Emit(Method.IsStatic ? OpCodes.Ldnull : OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldftn, clonedMethodRef);
            il.Emit(OpCodes.Newobj, _funcCtor);
            il.Emit(OpCodes.Callvirt, _dictionaryGetOrAdd);
            il.Emit(OpCodes.Ret);

            Method.Body.InitLocals = false;
            UpdateDebugInfo(Method);
            Method.Body.Optimize();
            UpdateDebugInfo(clonedMethod);
            clonedMethod.Body.Optimize();
        }

        private static MethodDefinition CloneMethod(MethodDefinition method)
        {
            var targetMethodName = "$_executor_" + method.Name;
            var isStaticMethod = method.IsStatic;
            var methodAttributes = MethodAttributes.Private;
            if (isStaticMethod)
                methodAttributes |= MethodAttributes.Static;

            var clonedMethod = new MethodDefinition(targetMethodName, methodAttributes, method.ReturnType)
            {
                AggressiveInlining = true, // try to get rid of additional stack frame
                HasThis = method.HasThis,
                ExplicitThis = method.ExplicitThis,
                CallingConvention = method.CallingConvention,
                Body = {InitLocals = method.Body.InitLocals}
            };

            foreach (var parameter in method.Parameters)
                clonedMethod.Parameters.Add(parameter);

            foreach (var variable in method.Body.Variables)
                clonedMethod.Body.Variables.Add(variable);

            foreach (var variable in method.Body.ExceptionHandlers)
                clonedMethod.Body.ExceptionHandlers.Add(variable);

            var targetProcessor = clonedMethod.Body.GetILProcessor();
            foreach (var instruction in method.Body.Instructions)
                targetProcessor.Append(instruction);

            if (method.HasGenericParameters)
            {
                //Contravariant:
                //  The generic type parameter is contravariant. A contravariant type parameter can appear as a parameter type in method signatures.  
                //Covariant:
                //  The generic type parameter is covariant. A covariant type parameter can appear as the result type of a method, the type of a read-only field, a declared base type, or an implemented interface. 
                //DefaultConstructorConstraint:
                //  A type can be substituted for the generic type parameter only if it has a parameterless constructor. 
                //None:
                //  There are no special flags. 
                //NotNullableValueTypeConstraint:
                //  A type can be substituted for the generic type parameter only if it is a value type and is not nullable. 
                //ReferenceTypeConstraint:
                //  A type can be substituted for the generic type parameter only if it is a reference type. 
                //SpecialConstraintMask:
                //  Selects the combination of all special constraint flags. This value is the result of using logical OR to combine the following flags: DefaultConstructorConstraint, ReferenceTypeConstraint, and NotNullableValueTypeConstraint. 
                //VarianceMask:
                //  Selects the combination of all variance flags. This value is the result of using logical OR to combine the following flags: Contravariant and Covariant. 
                foreach (var parameter in method.GenericParameters)
                {
                    var clonedParameter = new GenericParameter(parameter.Name, clonedMethod);
                    if (parameter.HasConstraints)
                    {
                        foreach (var parameterConstraint in parameter.Constraints)
                        {
                            clonedParameter.Attributes = parameter.Attributes;
                            clonedParameter.Constraints.Add(parameterConstraint);
                        }
                    }

                    if (parameter.HasReferenceTypeConstraint)
                    {
                        clonedParameter.Attributes |= GenericParameterAttributes.ReferenceTypeConstraint;
                        clonedParameter.HasReferenceTypeConstraint = true;
                    }

                    if (parameter.HasNotNullableValueTypeConstraint)
                    {
                        clonedParameter.Attributes |= GenericParameterAttributes.NotNullableValueTypeConstraint;
                        clonedParameter.HasNotNullableValueTypeConstraint = true;
                    }

                    if (parameter.HasDefaultConstructorConstraint)
                    {
                        clonedParameter.Attributes |= GenericParameterAttributes.DefaultConstructorConstraint;
                        clonedParameter.HasDefaultConstructorConstraint = true;
                    }

                    clonedMethod.GenericParameters.Add(clonedParameter);
                }
            }

            if (method.DebugInformation.HasSequencePoints)
            {
                foreach (var sequencePoint in method.DebugInformation.SequencePoints)
                    clonedMethod.DebugInformation.SequencePoints.Add(sequencePoint);
            }

            clonedMethod.DebugInformation.Scope = new ScopeDebugInformation(method.Body.Instructions.First(), method.Body.Instructions.Last());

            return clonedMethod;
        }

        private static void ClearMethod(MethodDefinition method)
        {
            var body = method.Body;
            body.Variables.Clear();
            body.Instructions.Clear();
            body.ExceptionHandlers.Clear();
            method.DebugInformation.SequencePoints.Clear();
        }

        public static TypeReference MakeGenericType(TypeReference self, params TypeReference [] arguments)
        {
            if (self.GenericParameters.Count != arguments.Length)
                throw new ArgumentException ();
           
            var instance = new GenericInstanceType (self);

            foreach (var argument in arguments)
                instance.GenericArguments.Add (argument);

            return instance;
        }

        public static MethodReference MakeGeneric(MethodReference self, params TypeReference [] arguments)
        {
            MethodReference baseReference = self.DeclaringType.Module.ImportReference(self);

            var reference = new GenericInstanceMethod(baseReference);
            
            foreach (var genericParameter in baseReference.GenericParameters)
                reference.GenericArguments.Add(genericParameter);

            return reference;
        }

        private static TypeReference FixTypeReference(TypeReference typeReference)
        {
            if (!typeReference.HasGenericParameters)
                return typeReference;

            // workaround for method in generic type
            // https://stackoverflow.com/questions/4968755/mono-cecil-call-generic-base-class-method-from-other-assembly
            var genericParameters = typeReference.GenericParameters
                .Select(x => x.GetElementType())
                .ToArray();

            return MakeGenericType(typeReference, genericParameters);
        }

        private static MethodReference FixMethodReference(TypeReference declaringType, MethodReference targetMethod)
        {
            // Taken and adapted from
            // https://stackoverflow.com/questions/4968755/mono-cecil-call-generic-base-class-method-from-other-assembly
            if (targetMethod is MethodDefinition)
            {
                var newTargetMethod = new MethodReference(targetMethod.Name, targetMethod.ReturnType, declaringType)
                {
                    HasThis = targetMethod.HasThis,
                    ExplicitThis = targetMethod.ExplicitThis,
                    CallingConvention = targetMethod.CallingConvention
                };
                foreach (var p in targetMethod.Parameters)
                    newTargetMethod.Parameters.Add(new ParameterDefinition(p.Name, p.Attributes, p.ParameterType));
                foreach (var gp in targetMethod.GenericParameters)
                    newTargetMethod.GenericParameters.Add(new GenericParameter(gp.Name, newTargetMethod));
                
                targetMethod = newTargetMethod;
            }
            else
                targetMethod.DeclaringType = declaringType;

            if (targetMethod.HasGenericParameters)
                return MakeGeneric(targetMethod);
            
            return targetMethod;
        }
        
        private static readonly FieldInfo _sequencePointOffsetFieldInfo = typeof(SequencePoint).GetField("offset", BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly FieldInfo _instructionOffsetInstructionFieldInfo = typeof(InstructionOffset).GetField("instruction", BindingFlags.Instance | BindingFlags.NonPublic);

        public static void UpdateDebugInfo(MethodDefinition method)
        {
            var debugInfo = method.DebugInformation;
            var instructions = method.Body.Instructions;
            var scope = debugInfo.Scope;

            if (scope == null || instructions.Count == 0)
            {
                return;
            }

            var oldSequencePoints = debugInfo.SequencePoints;
            var newSequencePoints = new Collection<SequencePoint>();

            // Step 1: check if all variables are present
            foreach (var variable in method.Body.Variables)
            {
                var hasVariable = scope.Variables.Any(x => x.Index == variable.Index);
                if (!hasVariable)
                {
                    var variableDebugInfo = new VariableDebugInformation(variable, $"__var_{variable.Index}");
                    scope.Variables.Add(variableDebugInfo);
                }
            }

            // Step 2: Make sure the instructions point to the correct items
            foreach (var oldSequencePoint in oldSequencePoints)
            {
                //var isValid = false;

                //// Special cases we need to ignore
                //if (oldSequencePoint.StartLine == AddressToIgnore ||
                //    oldSequencePoint.EndLine == AddressToIgnore)
                //{
                //    continue;
                //}

                var instructionOffset = (InstructionOffset)_sequencePointOffsetFieldInfo.GetValue(oldSequencePoint);
                var offsetInstruction = (Instruction)_instructionOffsetInstructionFieldInfo.GetValue(instructionOffset);

                // Fix offset
                for (var i = 0; i < instructions.Count; i++)
                {
                    var instruction = instructions[i];
                    if (instruction == offsetInstruction)
                    {
                        var newSequencePoint = new SequencePoint(instruction, oldSequencePoint.Document)
                        {
                            StartLine = oldSequencePoint.StartLine,
                            StartColumn = oldSequencePoint.StartColumn,
                            EndLine = oldSequencePoint.EndLine,
                            EndColumn = oldSequencePoint.EndColumn
                        };

                        newSequencePoints.Add(newSequencePoint);

                        //isValid = true;

                        break;
                    }
                }
            }

            debugInfo.SequencePoints.Clear();

            foreach (var newSequencePoint in newSequencePoints)
            {
                debugInfo.SequencePoints.Add(newSequencePoint);
            }

            // Step 3: update the scopes by setting the indices
            scope.Start = new InstructionOffset(instructions.First());
            scope.End = new InstructionOffset(instructions.Last());
        }
    }
}
