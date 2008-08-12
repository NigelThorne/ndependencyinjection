// #define DEBUG_TRANSFORM

namespace Reflector.CodeModel
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using Reflector.CodeModel;
    using Reflector.CodeModel.Memory;

    public class Transformer
    {
        public virtual IAssembly TransformAssembly(IAssembly value)
        {
            this.InsituTransformCustomAttributeCollection(value.Attributes);
            this.InsituTransformModuleCollection(value.Modules);
            return value;
        }

        public virtual IAssemblyReference TransformAssemblyReference(IAssemblyReference value)
        {
            return value;
        }

        public virtual IModule TransformModule(IModule value)
        {
            this.InsituTransformCustomAttributeCollection(value.Attributes);
            return value;
        }

        public virtual IModuleReference TransformModuleReference(IModuleReference value)
        {
            return value;
        }

        public virtual INamespace TransformNamespace(INamespace value)
        {
            this.InsituTransformTypeDeclarationCollection(value.Types);
            return value;
        }

        public virtual ITypeDeclaration TransformTypeDeclaration(ITypeDeclaration value)
        {
            this.InsituTransformCustomAttributeCollection(value.Attributes);
            this.InsituTransformMethodDeclarationCollection(value.Methods);
            this.InsituTransformFieldDeclarationCollection(value.Fields);
            this.InsituTransformPropertyDeclarationCollection(value.Properties);
            this.InsituTransformEventDeclarationCollection(value.Events);
            this.InsituTransformTypeDeclarationCollection(value.NestedTypes);
            return value;
        }

        public virtual ITypeReference TransformTypeReference(ITypeReference value)
        {
            this.InsituTransformTypeCollection(value.GenericArguments);
            return value;
        }

#if DEBUG_TRANSFORM
		private static int fileIndex = 0;
#endif

        public virtual IMethodDeclaration TransformMethodDeclaration(IMethodDeclaration value)
        {
            this.InsituTransformCustomAttributeCollection(value.Attributes);
            this.InsituTransformParameterDeclarationCollection(value.Parameters);
            this.InsituTransformMethodReferenceCollection(value.Overrides);
            value.ReturnType = this.TransformMethodReturnType(value.ReturnType);

            IBlockStatement blockStatement = (IBlockStatement)this.TransformStatement((IBlockStatement)value.Body);

#if DEBUG_TRANSFORM
			fileIndex++;
			using (StreamWriter writer = new StreamWriter(@"c:\Scratch\" + fileIndex.ToString("X4") + "-" + this.GetType().Name))
			{
				writer.Write(blockStatement);
			}
#endif

            value.Body = blockStatement;
            return value;
        }

        public virtual IMethodReference TransformMethodReference(IMethodReference value)
        {
            return value;
        }

        public virtual IFieldDeclaration TransformFieldDeclaration(IFieldDeclaration value)
        {
            this.InsituTransformCustomAttributeCollection(value.Attributes);
            return value;
        }

        public virtual IFieldReference TransformFieldReference(IFieldReference value)
        {
            return value;
        }

        public virtual IPropertyDeclaration TransformPropertyDeclaration(IPropertyDeclaration value)
        {
            this.InsituTransformCustomAttributeCollection(value.Attributes);
            return value;
        }

        public virtual IPropertyReference TransformPropertyReference(IPropertyReference value)
        {
            return value;
        }

        public virtual IEventDeclaration TransformEventDeclaration(IEventDeclaration value)
        {
            this.InsituTransformCustomAttributeCollection(value.Attributes);
            return value;
        }

        public virtual IEventReference TransformEventReference(IEventReference value)
        {
            return value;
        }

        public virtual IMethodReturnType TransformMethodReturnType(IMethodReturnType value)
        {
            this.InsituTransformCustomAttributeCollection(value.Attributes);
            return value;
        }

        public virtual IParameterDeclaration TransformParameterDeclaration(IParameterDeclaration value)
        {
            this.InsituTransformCustomAttributeCollection(value.Attributes);
            return value;
        }

        public virtual IMethodBody TransformMethodBody(IMethodBody value)
        {
            this.InsituTransformVariableDeclarationCollection(value.LocalVariables);
            this.InsituTransformInstructionCollection(value.Instructions);
            this.InsituTransformExceptionHandlerCollection(value.ExceptionHandlers);
            return value;
        }

        public virtual IInstruction TransformInstruction(IInstruction value)
        {
            return value;
        }

        public virtual IExceptionHandler TransformExceptionHandler(IExceptionHandler value)
        {
            return value;
        }

        public virtual IResource TransformResource(IResource value)
        {
            if (value is IEmbeddedResource)
            {
                return this.TransformEmbeddedResource(value as IEmbeddedResource);
            }

            if (value is IFileResource)
            {
                return this.TransformFileResource(value as IFileResource);
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid resource type '{0}'.", value.GetType().Name));
        }

        public virtual IResource TransformEmbeddedResource(IEmbeddedResource value)
        {
            return value;
        }

        public virtual IResource TransformFileResource(IFileResource value)
        {
            return value;
        }

        public virtual IUnmanagedResource TransformUnmanagedResource(IUnmanagedResource value)
        {
            return value;
        }

        public virtual IType TransformType(IType value)
        {
            if (value == null)
            {
                return null;
            }

            ITypeReference typeReference = value as ITypeReference;
            if (typeReference != null)
            {
                return this.TransformTypeReference(typeReference);
            }

            IArrayType arrayType = value as IArrayType;
            if (arrayType != null)
            {
                return this.TransformArrayType(arrayType);
            }

            IPointerType pointerType = value as IPointerType;
            if (pointerType != null)
            {
                return this.TransformPointerType(pointerType);
            }

            IReferenceType referenceType = value as IReferenceType;
            if (referenceType != null)
            {
                return this.TransformReferenceType(referenceType);
            }

            IOptionalModifier optionalModifier = value as IOptionalModifier;
            if (optionalModifier != null)
            {
                return this.TransformOptionalModifier(optionalModifier);
            }

            IRequiredModifier requiredModifier = value as IRequiredModifier;
            if (requiredModifier != null)
            {
                return this.TransformRequiredModifier(requiredModifier);
            }

            IFunctionPointer functionPointer = value as IFunctionPointer;
            if (functionPointer != null)
            {
                return this.TransformFunctionPointer(functionPointer);
            }

            IGenericParameter genericParameter = value as IGenericParameter;
            if (genericParameter != null)
            {
                return this.TransformGenericParameter(genericParameter);
            }

            IGenericArgument genericArgument = value as IGenericArgument;
            if (genericArgument != null)
            {
                return this.TransformGenericArgument(genericArgument);
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid type '{0}'.", value.GetType().Name));
        }

        public virtual ICustomAttribute TransformCustomAttribute(ICustomAttribute value)
        {
            this.InsituTransformExpressionCollection(value.Arguments);
            return value;
        }

        public virtual IStatement TransformStatement(IStatement value)
        {
            if (value == null)
            {
                return null;
            }

            // Performance: This method gets called often and needs to run fast.
            // 216988 ExpressionStatement
            // 123584 BlockStatement
            //  41587 ConditionStatement
            //  25567 MethodReturnStatement
            //  15264 LabeledStatement
            //  13260 GotoStatement

            if (value is IExpressionStatement)
            {
                return this.TransformExpressionStatement(value as IExpressionStatement);
            }

            if (value is IBlockStatement)
            {
                return this.TransformBlockStatement(value as IBlockStatement);
            }

            if (value is IConditionStatement)
            {
                return this.TransformConditionStatement(value as IConditionStatement);
            }

            if (value is IMethodReturnStatement)
            {
                return this.TransformMethodReturnStatement(value as IMethodReturnStatement);
            }

            if (value is ILabeledStatement)
            {
                return this.TransformLabeledStatement(value as ILabeledStatement);
            }

            if (value is IGotoStatement)
            {
                return this.TransformGotoStatement(value as IGotoStatement);
            }

            if (value is IForStatement)
            {
                return this.TransformForStatement(value as IForStatement);
            }

            if (value is IForEachStatement)
            {
                return this.TransformForEachStatement(value as IForEachStatement);
            }

            if (value is IWhileStatement)
            {
                return this.TransformWhileStatement(value as IWhileStatement);
            }

            if (value is IDoStatement)
            {
                return this.TransformDoStatement(value as IDoStatement);
            }

            if (value is ITryCatchFinallyStatement)
            {
                return this.TransformTryCatchFinallyStatement(value as ITryCatchFinallyStatement);
            }

            if (value is IThrowExceptionStatement)
            {
                return this.TransformThrowExceptionStatement(value as IThrowExceptionStatement);
            }

            if (value is IAttachEventStatement)
            {
                return this.TransformAttachEventStatement(value as IAttachEventStatement);
            }

            if (value is IRemoveEventStatement)
            {
                return this.TransformRemoveEventStatement(value as IRemoveEventStatement);
            }

            if (value is ISwitchStatement)
            {
                return this.TransformSwitchStatement(value as ISwitchStatement);
            }

            if (value is IBreakStatement)
            {
                return this.TransformBreakStatement(value as IBreakStatement);
            }

            if (value is IContinueStatement)
            {
                return this.TransformContinueStatement(value as IContinueStatement);
            }

            if (value is ICommentStatement)
            {
                return this.TransformCommentStatement(value as ICommentStatement);
            }

            if (value is IUsingStatement)
            {
                return this.TransformUsingStatement(value as IUsingStatement);
            }

            if (value is IFixedStatement)
            {
                return this.TransformFixedStatement(value as IFixedStatement);
            }

            if (value is ILockStatement)
            {
                return this.TransformLockStatement(value as ILockStatement);
            }

            if (value is IMemoryCopyStatement)
            {
                return this.TransformMemoryCopyStatement(value as IMemoryCopyStatement);
            }

            if (value is IMemoryInitializeStatement)
            {
                return this.TransformMemoryInitializeStatement(value as IMemoryInitializeStatement);
            }

            if (value is IDebugBreakStatement)
            {
                return this.TransformDebugBreakStatement(value as IDebugBreakStatement);
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid statement type '{0}'.", value.GetType().Name));
        }

        public virtual IStatement TransformBlockStatement(IBlockStatement value)
        {
            this.InsituTransformStatementCollection(value.Statements);
            return value;
        }

        public virtual IStatement TransformCommentStatement(ICommentStatement value)
        {
            return value;
        }

        public virtual IStatement TransformMethodReturnStatement(IMethodReturnStatement value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IStatement TransformConditionStatement(IConditionStatement value)
        {
            value.Condition = this.TransformExpression(value.Condition);
            value.Then = (IBlockStatement)this.TransformStatement(value.Then);
            value.Else = (IBlockStatement)this.TransformStatement(value.Else);
            return value;
        }

        public virtual IStatement TransformTryCatchFinallyStatement(ITryCatchFinallyStatement value)
        {
            value.Try = (IBlockStatement)this.TransformStatement(value.Try);
            this.InsituTransformCatchClauseCollection(value.CatchClauses);
            value.Finally = (IBlockStatement)this.TransformStatement(value.Finally);
            value.Fault = (IBlockStatement)this.TransformStatement(value.Fault);
            return value;
        }

        public virtual IExpression TransformAssignExpression(IAssignExpression value)
        {
            value.Target = this.TransformExpression(value.Target);
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IStatement TransformExpressionStatement(IExpressionStatement value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return (value.Expression != null) ? value : null;
        }

        public virtual IStatement TransformForStatement(IForStatement value)
        {
            value.Initializer = this.TransformStatement(value.Initializer);
            value.Condition = this.TransformExpression(value.Condition);
            value.Increment = this.TransformStatement(value.Increment);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual IStatement TransformForEachStatement(IForEachStatement value)
        {
            value.Variable = this.TransformVariableDeclaration(value.Variable);
            value.Expression = this.TransformExpression(value.Expression);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual IStatement TransformUsingStatement(IUsingStatement value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual IStatement TransformFixedStatement(IFixedStatement value)
        {
            value.Variable = this.TransformVariableDeclaration(value.Variable);
            value.Expression = this.TransformExpression(value.Expression);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual IStatement TransformLockStatement(ILockStatement value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual IStatement TransformWhileStatement(IWhileStatement value)
        {
            value.Condition = this.TransformExpression(value.Condition);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual IStatement TransformDoStatement(IDoStatement value)
        {
            value.Condition = this.TransformExpression(value.Condition);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual IStatement TransformBreakStatement(IBreakStatement value)
        {
            return value;
        }

        public virtual IStatement TransformContinueStatement(IContinueStatement value)
        {
            return value;
        }

        public virtual IStatement TransformThrowExceptionStatement(IThrowExceptionStatement value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformVariableDeclarationExpression(IVariableDeclarationExpression value)
        {
            value.Variable = this.TransformVariableDeclaration(value.Variable);
            return value;
        }

        public virtual IStatement TransformAttachEventStatement(IAttachEventStatement value)
        {
            value.Event = (IEventReferenceExpression)this.TransformExpression(value.Event);
            value.Listener = this.TransformExpression(value.Listener);
            return value;
        }

        public virtual IStatement TransformRemoveEventStatement(IRemoveEventStatement value)
        {
            value.Event = (IEventReferenceExpression)this.TransformExpression(value.Event);
            value.Listener = this.TransformExpression(value.Listener);
            return value;
        }

        public virtual IStatement TransformSwitchStatement(ISwitchStatement value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            this.InsituTransformSwitchCaseCollection(value.Cases);
            return value;
        }

        public virtual IStatement TransformGotoStatement(IGotoStatement value)
        {
            return value;
        }

        public virtual IStatement TransformLabeledStatement(ILabeledStatement value)
        {
            value.Statement = this.TransformStatement(value.Statement);
            return value;
        }

        public virtual IStatement TransformMemoryCopyStatement(IMemoryCopyStatement value)
        {
            value.Source = this.TransformExpression(value.Source);
            value.Destination = this.TransformExpression(value.Destination);
            value.Length = this.TransformExpression(value.Length);
            return value;
        }

        public virtual IStatement TransformMemoryInitializeStatement(IMemoryInitializeStatement value)
        {
            value.Offset = this.TransformExpression(value.Offset);
            value.Value = this.TransformExpression(value.Value);
            value.Length = this.TransformExpression(value.Length);
            return value;
        }

        public virtual IStatement TransformDebugBreakStatement(IDebugBreakStatement value)
        {
            return value;
        }

        public virtual IExpression TransformExpression(IExpression value)
        {
            if (value == null)
            {
                return null;
            }

            // Performance: This method gets called often and needs to run fast.
            // 200322 VariableReferenceExpression
            // 110420 LiteralExpression
            // 115235 FieldReferenceExpression
            // 102083 PropertyReferenceExpression
            //  96179 AssignExpression
            //  91672 BinaryExpression
            //  82433 ThisReferenceExpression
            //  73535 MethodInvokeExpression
            //  71393 MethodReferenceExpression
            //  70042 ArgumentReferenceExpression
            //  56359 VariableDeclarationExpression
            //  48735 TypeReferenceExpression
            //  23987 BaseReferenceExpression

            if (value is IVariableReferenceExpression)
            {
                return this.TransformVariableReferenceExpression(value as IVariableReferenceExpression);
            }

            if (value is ILiteralExpression)
            {
                return this.TransformLiteralExpression(value as ILiteralExpression);
            }

            if (value is IFieldReferenceExpression)
            {
                return this.TransformFieldReferenceExpression(value as IFieldReferenceExpression);
            }

            if (value is IPropertyReferenceExpression)
            {
                return this.TransformPropertyReferenceExpression(value as IPropertyReferenceExpression);
            }

            if (value is IAssignExpression)
            {
                return this.TransformAssignExpression(value as IAssignExpression);
            }

            if (value is IBinaryExpression)
            {
                return this.TransformBinaryExpression(value as IBinaryExpression);
            }

            if (value is IThisReferenceExpression)
            {
                return this.TransformThisReferenceExpression(value as IThisReferenceExpression);
            }

            if (value is IMethodInvokeExpression)
            {
                return this.TransformMethodInvokeExpression(value as IMethodInvokeExpression);
            }

            if (value is IMethodReferenceExpression)
            {
                return this.TransformMethodReferenceExpression(value as IMethodReferenceExpression);
            }

            if (value is IArgumentReferenceExpression)
            {
                return this.TransformArgumentReferenceExpression(value as IArgumentReferenceExpression);
            }

            if (value is IVariableDeclarationExpression)
            {
                return this.TransformVariableDeclarationExpression(value as IVariableDeclarationExpression);
            }

            if (value is ITypeReferenceExpression)
            {
                return this.TransformTypeReferenceExpression(value as ITypeReferenceExpression);
            }

            if (value is IBaseReferenceExpression)
            {
                return this.TransformBaseReferenceExpression(value as IBaseReferenceExpression);
            }

            if (value is IUnaryExpression)
            {
                return this.TransformUnaryExpression(value as IUnaryExpression);
            }

            if (value is ITryCastExpression)
            {
                return this.TransformTryCastExpression(value as ITryCastExpression);
            }

            if (value is ICanCastExpression)
            {
                return this.TransformCanCastExpression(value as ICanCastExpression);
            }

            if (value is ICastExpression)
            {
                return this.TransformCastExpression(value as ICastExpression);
            }

            if (value is ITypeOfExpression)
            {
                return this.TransformTypeOfExpression(value as ITypeOfExpression);
            }

            if (value is IFieldOfExpression)
            {
                return this.TransformFieldOfExpression(value as IFieldOfExpression);
            }

            if (value is IMethodOfExpression)
            {
                return this.TransformMethodOfExpression(value as IMethodOfExpression);
            }

            if (value is IMemberInitializerExpression)
            {
                return this.TransformMemberInitializerExpression(value as IMemberInitializerExpression);
            }

            if (value is IEventReferenceExpression)
            {
                return this.TransformEventReferenceExpression(value as IEventReferenceExpression);
            }

            if (value is IArgumentListExpression)
            {
                return this.TransformArgumentListExpression(value as IArgumentListExpression);
            }

            if (value is IArrayCreateExpression)
            {
                return this.TransformArrayCreateExpression(value as IArrayCreateExpression);
            }

            if (value is IBlockExpression)
            {
                return this.TransformBlockExpression(value as IBlockExpression);
            }

            if (value is IConditionExpression)
            {
                return this.TransformConditionExpression(value as IConditionExpression);
            }

            if (value is INullCoalescingExpression)
            {
                return this.TransformNullCoalescingExpression(value as INullCoalescingExpression);
            }

            if (value is IDelegateCreateExpression)
            {
                return this.TransformDelegateCreateExpression(value as IDelegateCreateExpression);
            }

            if (value is IAnonymousMethodExpression)
            {
                return this.TransformAnonymousMethodExpression(value as IAnonymousMethodExpression);
            }

            if (value is IPropertyIndexerExpression)
            {
                return this.TransformPropertyIndexerExpression(value as IPropertyIndexerExpression);
            }

            if (value is IArrayIndexerExpression)
            {
                return this.TransformArrayIndexerExpression(value as IArrayIndexerExpression);
            }

            if (value is IDelegateInvokeExpression)
            {
                return this.TransformDelegateInvokeExpression(value as IDelegateInvokeExpression);
            }

            if (value is IObjectCreateExpression)
            {
                return this.TransformObjectCreateExpression(value as IObjectCreateExpression);
            }

            if (value is IAddressOfExpression)
            {
                return this.TransformAddressOfExpression(value as IAddressOfExpression);
            }

            if (value is IAddressReferenceExpression)
            {
                return this.TransformAddressReferenceExpression(value as IAddressReferenceExpression);
            }

            if (value is IAddressOutExpression)
            {
                return this.TransformAddressOutExpression(value as IAddressOutExpression);
            }

            if (value is IAddressDereferenceExpression)
            {
                return this.TransformAddressDereferenceExpression(value as IAddressDereferenceExpression);
            }

            if (value is ISizeOfExpression)
            {
                return this.TransformSizeOfExpression(value as ISizeOfExpression);
            }

            if (value is ITypedReferenceCreateExpression)
            {
                return this.TransformTypedReferenceCreateExpression(value as ITypedReferenceCreateExpression);
            }

            if (value is ITypeOfTypedReferenceExpression)
            {
                return this.TransformTypeOfTypedReferenceExpression(value as ITypeOfTypedReferenceExpression);
            }

            if (value is IValueOfTypedReferenceExpression)
            {
                return this.TransformValueOfTypedReferenceExpression(value as IValueOfTypedReferenceExpression);
            }

            if (value is IStackAllocateExpression)
            {
                return this.TransformStackAllocateExpression(value as IStackAllocateExpression);
            }

            if (value is IGenericDefaultExpression)
            {
                return this.TransformGenericDefaultExpression(value as IGenericDefaultExpression);
            }

            if (value is IQueryExpression)
            {
                return this.TransformQueryExpression(value as IQueryExpression);
            }

            if (value is ILambdaExpression)
            {
                return this.TransformLambdaExpression(value as ILambdaExpression);
            }

            if (value is ISnippetExpression)
            {
                return this.TransformSnippetExpression(value as ISnippetExpression);
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid expression type '{0}'.", value.GetType().Name));
        }

        public virtual IExpression TransformMemberInitializerExpression(IMemberInitializerExpression value)
        {
            value.Value = this.TransformExpression(value.Value);
            return value;
        }

        public virtual IExpression TransformTypeOfExpression(ITypeOfExpression value)
        {
            return value;
        }

        public virtual IExpression TransformFieldOfExpression(IFieldOfExpression value)
        {
            return value;
        }

        public virtual IExpression TransformMethodOfExpression(IMethodOfExpression value)
        {
            if (value.Type != null)
            {
                value.Type = this.TransformTypeReference(value.Type);
            }
            return value;
        }

        public virtual IExpression TransformArrayCreateExpression(IArrayCreateExpression value)
        {
            this.InsituTransformExpressionCollection(value.Dimensions);
            value.Initializer = (IBlockExpression)this.TransformExpression(value.Initializer);
            value.Type = this.TransformType(value.Type);
            return value;
        }

        public virtual IExpression TransformBlockExpression(IBlockExpression value)
        {
            this.InsituTransformExpressionCollection(value.Expressions);
            return value;
        }

        public virtual IExpression TransformBaseReferenceExpression(IBaseReferenceExpression value)
        {
            return value;
        }

        public virtual IExpression TransformTryCastExpression(ITryCastExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformCanCastExpression(ICanCastExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformCastExpression(ICastExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformConditionExpression(IConditionExpression value)
        {
            value.Condition = this.TransformExpression(value.Condition);
            value.Then = this.TransformExpression(value.Then);
            value.Else = this.TransformExpression(value.Else);
            return value;
        }

        public virtual IExpression TransformNullCoalescingExpression(INullCoalescingExpression value)
        {
            value.Condition = this.TransformExpression(value.Condition);
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformDelegateCreateExpression(IDelegateCreateExpression value)
        {
            value.Target = this.TransformExpression(value.Target);
            return value;
        }

        public virtual IExpression TransformAnonymousMethodExpression(IAnonymousMethodExpression value)
        {
            value.DelegateType = this.TransformType(value.DelegateType);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            value.ReturnType = this.TransformMethodReturnType(value.ReturnType);
            this.InsituTransformParameterDeclarationCollection(value.Parameters);
            return value;
        }

        public virtual IExpression TransformQueryExpression(IQueryExpression value)
        {
            value.From = this.TransformFromClause(value.From);
            value.Body = this.TransformQueryBody(value.Body);
            return value;
        }

        public virtual IQueryBody TransformQueryBody(IQueryBody value)
        {
            value.Operation = this.TransformQueryOperation(value.Operation);

            this.InsituTransformQueryClauseCollection(value.Clauses);

            if (value.Continuation != null)
            {
                value.Continuation = this.TransformQueryContinuation(value.Continuation);
            }

            return value;
        }

        public virtual IQueryClause TransformQueryClause(IQueryClause value)
        {
            if (value is IWhereClause)
            {
                return this.TransformWhereClause(value as IWhereClause);
            }

            if (value is ILetClause)
            {
                return this.TransformLetClause(value as ILetClause);
            }

            if (value is IFromClause)
            {
                return this.TransformFromClause(value as IFromClause);
            }

            if (value is IJoinClause)
            {
                return this.TransformJoinClause(value as IJoinClause);
            }

            if (value is IOrderClause)
            {
                return this.TransformOrderClause(value as IOrderClause);
            }

            throw new NotSupportedException();
        }

        public virtual IQueryOperation TransformQueryOperation(IQueryOperation value)
        {
            if (value is ISelectOperation)
            {
                return this.TransformSelectOperation(value as ISelectOperation);
            }

            if (value is IGroupOperation)
            {
                return this.TransformGroupOperation(value as IGroupOperation);
            }

            throw new NotSupportedException();
        }

        public virtual IQueryContinuation TransformQueryContinuation(IQueryContinuation value)
        {
            value.Variable = this.TransformVariableDeclaration(value.Variable);
            value.Body = this.TransformQueryBody(value.Body);
            return value;
        }

        public virtual ISelectOperation TransformSelectOperation(ISelectOperation value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IGroupOperation TransformGroupOperation(IGroupOperation value)
        {
            value.Item = this.TransformExpression(value.Item);
            value.Key = this.TransformExpression(value.Key);
            return value;
        }

        public virtual IWhereClause TransformWhereClause(IWhereClause value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual ILetClause TransformLetClause(ILetClause value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IFromClause TransformFromClause(IFromClause value)
        {
            value.Variable = this.TransformVariableDeclaration(value.Variable);
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IJoinClause TransformJoinClause(IJoinClause value)
        {
            value.Variable = this.TransformVariableDeclaration(value.Variable);
            value.In = this.TransformExpression(value.In);
            value.On = this.TransformExpression(value.On);
            value.Equality = this.TransformExpression(value.Equality);
            if (value.Into != null)
            {
                value.Into = this.TransformVariableDeclaration(value.Into);
            }
            return value;
        }

        public virtual IOrderClause TransformOrderClause(IOrderClause value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformLambdaExpression(ILambdaExpression value)
        {
            this.InisituTransformVariableDeclarationCollection(value.Parameters);
            value.Body = this.TransformExpression(value.Body);
            return value;
        }

        public virtual IExpression TransformTypeReferenceExpression(ITypeReferenceExpression value)
        {
            return value;
        }

        public virtual IExpression TransformFieldReferenceExpression(IFieldReferenceExpression value)
        {
            value.Target = this.TransformExpression(value.Target);
            return value;
        }

        public virtual IExpression TransformArgumentReferenceExpression(IArgumentReferenceExpression value)
        {
            return value;
        }

        public virtual IExpression TransformArgumentListExpression(IArgumentListExpression value)
        {
            return value;
        }

        public virtual IExpression TransformVariableReferenceExpression(IVariableReferenceExpression value)
        {
            value.Variable = this.TransformVariableReference(value.Variable);
            return value;
        }

        public virtual IExpression TransformPropertyIndexerExpression(IPropertyIndexerExpression value)
        {
            this.InsituTransformExpressionCollection(value.Indices);
            value.Target = (IPropertyReferenceExpression)this.TransformExpression(value.Target);
            return value;
        }

        public virtual IExpression TransformArrayIndexerExpression(IArrayIndexerExpression value)
        {
            this.InsituTransformExpressionCollection(value.Indices);
            value.Target = this.TransformExpression(value.Target);
            return value;
        }

        public virtual IExpression TransformMethodInvokeExpression(IMethodInvokeExpression value)
        {
            this.InsituTransformExpressionCollection(value.Arguments);
            value.Method = this.TransformExpression(value.Method);
            return value;
        }

        public virtual IExpression TransformMethodReferenceExpression(IMethodReferenceExpression value)
        {
            value.Target = this.TransformExpression(value.Target);
            return value;
        }

        public virtual IExpression TransformEventReferenceExpression(IEventReferenceExpression value)
        {
            value.Target = this.TransformExpression(value.Target);
            return value;
        }

        public virtual IExpression TransformDelegateInvokeExpression(IDelegateInvokeExpression value)
        {
            this.InsituTransformExpressionCollection(value.Arguments);
            value.Target = this.TransformExpression(value.Target);
            return value;
        }

        public virtual IExpression TransformObjectCreateExpression(IObjectCreateExpression value)
        {
            this.InsituTransformExpressionCollection(value.Arguments);

            if (value.Initializer != null)
            {
                value.Initializer = (IBlockExpression)this.TransformBlockExpression(value.Initializer);
            }

            return value;
        }

        public virtual IExpression TransformPropertyReferenceExpression(IPropertyReferenceExpression value)
        {
            value.Target = this.TransformExpression(value.Target);
            return value;
        }

        public virtual IExpression TransformThisReferenceExpression(IThisReferenceExpression value)
        {
            return value;
        }

        public virtual IExpression TransformAddressOfExpression(IAddressOfExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformAddressReferenceExpression(IAddressReferenceExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformAddressOutExpression(IAddressOutExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformAddressDereferenceExpression(IAddressDereferenceExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformSizeOfExpression(ISizeOfExpression value)
        {
            return value;
        }

        public virtual IExpression TransformStackAllocateExpression(IStackAllocateExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformUnaryExpression(IUnaryExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformBinaryExpression(IBinaryExpression value)
        {
            value.Left = this.TransformExpression(value.Left);
            value.Right = this.TransformExpression(value.Right);
            return value;
        }

        public virtual IExpression TransformLiteralExpression(ILiteralExpression value)
        {
            return value;
        }

        public virtual IExpression TransformGenericDefaultExpression(IGenericDefaultExpression value)
        {
            return value;
        }

        public virtual IExpression TransformTypeOfTypedReferenceExpression(ITypeOfTypedReferenceExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformValueOfTypedReferenceExpression(IValueOfTypedReferenceExpression value)
        {
            value.TargetType = this.TransformType(value.TargetType);
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformTypedReferenceCreateExpression(ITypedReferenceCreateExpression value)
        {
            value.Expression = this.TransformExpression(value.Expression);
            return value;
        }

        public virtual IExpression TransformSnippetExpression(ISnippetExpression value)
        {
            return value;
        }

        public virtual ISwitchCase TransformSwitchCase(ISwitchCase value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is IDefaultCase)
            {
                return this.TransformDefaultCase(value as IDefaultCase);
            }

            if (value is IConditionCase)
            {
                return this.TransformConditionCase(value as IConditionCase);
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid switch case type '{0}'.", value.GetType().Name));
        }

        public virtual IConditionCase TransformConditionCase(IConditionCase value)
        {
            value.Condition = this.TransformExpression(value.Condition);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual IDefaultCase TransformDefaultCase(IDefaultCase value)
        {
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual ICatchClause TransformCatchClause(ICatchClause value)
        {
            value.Variable = this.TransformVariableDeclaration(value.Variable);
            value.Condition = this.TransformExpression(value.Condition);
            value.Body = (IBlockStatement)this.TransformStatement(value.Body);
            return value;
        }

        public virtual IVariableDeclaration TransformVariableDeclaration(IVariableDeclaration value)
        {
            return value;
        }

        public virtual IVariableReference TransformVariableReference(IVariableReference value)
        {
            return value;
        }

        public virtual IType TransformOptionalModifier(IOptionalModifier value)
        {
            value.ElementType = this.TransformType(value.ElementType);
            value.Modifier = (ITypeReference)this.TransformTypeReference(value.Modifier);
            return value;
        }

        public virtual IType TransformRequiredModifier(IRequiredModifier value)
        {
            value.ElementType = this.TransformType(value.ElementType);
            value.Modifier = (ITypeReference)this.TransformTypeReference(value.Modifier);
            return value;
        }

        public virtual IType TransformPointerType(IPointerType value)
        {
            value.ElementType = this.TransformType(value.ElementType);
            return value;
        }

        public virtual IType TransformReferenceType(IReferenceType value)
        {
            value.ElementType = this.TransformType(value.ElementType);
            return value;
        }

        public virtual IType TransformArrayType(IArrayType value)
        {
            /* value.ElementType */
            this.TransformType(value.ElementType);
            /* value.Dimensions.AddRange( */
            this.TransformArrayDimensionCollection(value.Dimensions) /* ) */ ;
            return value;
        }

        public virtual IType TransformFunctionPointer(IFunctionPointer value)
        {
            return value;
        }

        public virtual IType TransformGenericParameter(IGenericParameter value)
        {
            return value;
        }

        public virtual IType TransformGenericArgument(IGenericArgument value)
        {
            return value;
        }

        public virtual IArrayDimension TransformArrayDimension(IArrayDimension value)
        {
            return value;
        }

        public virtual IModuleCollection TransformModuleCollection(IModuleCollection value)
        {
            IModule[] array = new IModule[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformModule(value[i]);
            }

            IModuleCollection target = new ModuleCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IAssemblyReferenceCollection TransformAssemblyReferenceCollection(IAssemblyReferenceCollection value)
        {
            IAssemblyReference[] array = new IAssemblyReference[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformAssemblyReference(value[i]);
            }

            IAssemblyReferenceCollection target = new AssemblyReferenceCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IModuleReferenceCollection TransformModuleReferenceCollection(IModuleReferenceCollection value)
        {
            IModuleReference[] array = new IModuleReference[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformModuleReference(value[i]);
            }

            IModuleReferenceCollection target = new ModuleReferenceCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IResourceCollection TransformResourceCollection(IResourceCollection value)
        {
            IResource[] array = new IResource[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformResource(value[i]);
            }

            IResourceCollection target = new ResourceCollection();
            target.AddRange(array);
            return target;
        }

        public virtual ITypeDeclarationCollection TransformTypeDeclarationCollection(ITypeDeclarationCollection value)
        {
            ITypeDeclaration[] array = new ITypeDeclaration[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformTypeDeclaration(value[i]);
            }

            ITypeDeclarationCollection target = new TypeDeclarationCollection();
            target.AddRange(array);
            return target;
        }

        public virtual ITypeCollection TransformTypeCollection(ITypeCollection types)
        {
            IType[] array = new IType[types.Count];
            for (int i = 0; i < types.Count; i++)
            {
                array[i] = this.TransformType(types[i]);
            }

            ITypeCollection target = new TypeCollection();
            target.AddRange(array);
            return target;
        }

        public virtual ITypeReferenceCollection TransformTypeReferenceCollection(ITypeReferenceCollection types)
        {
            ITypeReference[] array = new ITypeReference[types.Count];
            for (int i = 0; i < types.Count; i++)
            {
                array[i] = this.TransformTypeReference(types[i]);
            }

            ITypeReferenceCollection target = new TypeReferenceCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IFieldDeclarationCollection TransformFieldDeclarationCollection(IFieldDeclarationCollection fields)
        {
            IFieldDeclaration[] array = new IFieldDeclaration[fields.Count];
            for (int i = 0; i < fields.Count; i++)
            {
                array[i] = this.TransformFieldDeclaration(fields[i]);
            }

            IFieldDeclarationCollection target = new FieldDeclarationCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IMethodDeclarationCollection TransformMethodDeclarationCollection(IMethodDeclarationCollection methods)
        {
            IMethodDeclaration[] array = new IMethodDeclaration[methods.Count];
            for (int i = 0; i < methods.Count; i++)
            {
                array[i] = this.TransformMethodDeclaration(methods[i]);
            }

            IMethodDeclarationCollection target = new MethodDeclarationCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IPropertyDeclarationCollection TransformPropertyDeclarationCollection(IPropertyDeclarationCollection properties)
        {
            IPropertyDeclaration[] array = new IPropertyDeclaration[properties.Count];
            for (int i = 0; i < properties.Count; i++)
            {
                array[i] = this.TransformPropertyDeclaration(properties[i]);
            }

            IPropertyDeclarationCollection target = new PropertyDeclarationCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IEventDeclarationCollection TransformEventDeclarationCollection(IEventDeclarationCollection events)
        {
            IEventDeclaration[] array = new IEventDeclaration[events.Count];
            for (int i = 0; i < events.Count; i++)
            {
                array[i] = this.TransformEventDeclaration(events[i]);
            }

            IEventDeclarationCollection target = new EventDeclarationCollection();
            target.AddRange(array);
            return target;
        }

        public virtual ICustomAttributeCollection TransformCustomAttributeCollection(ICustomAttributeCollection attributes)
        {
            ICustomAttribute[] array = new ICustomAttribute[attributes.Count];
            for (int i = 0; i < attributes.Count; i++)
            {
                array[i] = this.TransformCustomAttribute(attributes[i]);
            }

            ICustomAttributeCollection target = new CustomAttributeCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IParameterDeclarationCollection TransformParameterDeclarationCollection(IParameterDeclarationCollection parameters)
        {
            IParameterDeclaration[] array = new IParameterDeclaration[parameters.Count];
            for (int i = 0; i < parameters.Count; i++)
            {
                array[i] = this.TransformParameterDeclaration(parameters[i]);
            }

            IParameterDeclarationCollection target = new ParameterDeclarationCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IMethodReferenceCollection TransformMethodReferenceCollection(IMethodReferenceCollection parameters)
        {
            IMethodReference[] array = new IMethodReference[parameters.Count];
            for (int i = 0; i < parameters.Count; i++)
            {
                array[i] = this.TransformMethodReference(parameters[i]);
            }

            IMethodReferenceCollection target = new MethodReferenceCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IStatementCollection TransformStatementCollection(IStatementCollection value)
        {
            int count = value.Count;
            IStatement[] array = new IStatement[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = this.TransformStatement(value[i]);
            }

            IStatementCollection target = new StatementCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IExpressionCollection TransformExpressionCollection(IExpressionCollection expressions)
        {
            IExpression[] array = new IExpression[expressions.Count];
            for (int i = 0; i < expressions.Count; i++)
            {
                array[i] = this.TransformExpression(expressions[i]);
            }

            IExpressionCollection target = new ExpressionCollection();
            target.AddRange(array);
            return target;
        }

        public virtual ICatchClauseCollection TransformCatchClauseCollection(ICatchClauseCollection value)
        {
            ICatchClause[] array = new ICatchClause[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformCatchClause(value[i]);
            }

            ICatchClauseCollection target = new CatchClauseCollection();
            target.AddRange(array);
            return target;
        }

        public virtual ISwitchCaseCollection TransformSwitchCaseCollection(ISwitchCaseCollection value)
        {
            ISwitchCase[] array = new ISwitchCase[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformSwitchCase(value[i]);
            }

            ISwitchCaseCollection target = new SwitchCaseCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IQueryClauseCollection TransformQueryClauseCollection(IQueryClauseCollection value)
        {
            IQueryClause[] array = new IQueryClause[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformQueryClause(value[i]);
            }

            IQueryClauseCollection target = new QueryClauseCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IArrayDimensionCollection TransformArrayDimensionCollection(IArrayDimensionCollection value)
        {
            IArrayDimension[] array = new IArrayDimension[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformArrayDimension(value[i]);
            }

            IArrayDimensionCollection target = new ArrayDimensionCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IUnmanagedResourceCollection TransformUnmanagedResourceCollection(IUnmanagedResourceCollection value)
        {
            IUnmanagedResource[] array = new IUnmanagedResource[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformUnmanagedResource(value[i]);
            }

            IUnmanagedResourceCollection target = new UnmanagedResourceCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IInstructionCollection TransformInstructionCollection(IInstructionCollection value)
        {
            IInstruction[] array = new IInstruction[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformInstruction(value[i]);
            }

            IInstructionCollection target = new InstructionCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IExceptionHandlerCollection TransformExceptionHandlerCollection(IExceptionHandlerCollection value)
        {
            IExceptionHandler[] array = new IExceptionHandler[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformExceptionHandler(value[i]);
            }

            IExceptionHandlerCollection target = new ExceptionHandlerCollection();
            target.AddRange(array);
            return target;
        }

        public virtual IVariableDeclarationCollection TransformVariableDeclarationCollection(IVariableDeclarationCollection value)
        {
            IVariableDeclaration[] array = new IVariableDeclaration[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                array[i] = this.TransformVariableDeclaration(value[i]);
            }

            IVariableDeclarationCollection target = new VariableDeclarationCollection();
            target.AddRange(array);
            return target;
        }

        private void InsituTransformModuleCollection(IModuleCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformModule(value[i]);
            }
        }

        /*
        private void InsituTransformResourceCollection(IResourceCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformResource(value[i]);
            }
        }
        */

        /*
        private void InsituTypeCollection(ITypeCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformType(value[i]);
            }
        }
        */

        private void InsituTransformTypeDeclarationCollection(ITypeDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformTypeDeclaration(value[i]);
            }
        }

        private void InsituTransformFieldDeclarationCollection(IFieldDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformFieldDeclaration(value[i]);
            }
        }

        private void InsituTransformMethodDeclarationCollection(IMethodDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformMethodDeclaration(value[i]);
            }
        }

        private void InsituTransformPropertyDeclarationCollection(IPropertyDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformPropertyDeclaration(value[i]);
            }
        }

        private void InsituTransformEventDeclarationCollection(IEventDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformEventDeclaration(value[i]);
            }
        }

        private void InsituTransformCustomAttributeCollection(ICustomAttributeCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformCustomAttribute(value[i]);
            }
        }

        private void InsituTransformParameterDeclarationCollection(IParameterDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformParameterDeclaration(value[i]);
            }
        }

        private void InsituTransformMethodReferenceCollection(IMethodReferenceCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformMethodReference(value[i]);
            }
        }

        private void InsituTransformStatementCollection(IStatementCollection value)
        {
            int index = 0;
            while (index < value.Count)
            {
                IStatement statement = this.TransformStatement(value[index]);
                if (statement != null)
                {
                    value[index++] = statement;
                }
                else
                {
                    value.RemoveAt(index);
                }
            }
        }

        private void InsituTransformExpressionCollection(IExpressionCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformExpression(value[i]);
            }
        }

        private void InisituTransformVariableDeclarationCollection(IVariableDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformVariableDeclaration(value[i]);
            }
        }

        private void InsituTransformCatchClauseCollection(ICatchClauseCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformCatchClause(value[i]);
            }
        }

        private void InsituTransformSwitchCaseCollection(ISwitchCaseCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformSwitchCase(value[i]);
            }
        }

        private void InsituTransformQueryClauseCollection(IQueryClauseCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformQueryClause(value[i]);
            }
        }

        private void InsituTransformTypeCollection(ITypeCollection value)
        {
            // for (int i = 0; i < value.Count; i++)
            {
                // value[i] = this.TransformType(value[i]);
            }
        }

        private void InsituTransformVariableDeclarationCollection(IVariableDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformVariableDeclaration(value[i]);
            }
        }

        private void InsituTransformInstructionCollection(IInstructionCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformInstruction(value[i]);
            }
        }

        private void InsituTransformExceptionHandlerCollection(IExceptionHandlerCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformExceptionHandler(value[i]);
            }
        }

        /*
        private void InsituTransformArrayDimensionCollection(IArrayDimensionCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                value[i] = this.TransformArrayDimension(value[i]);
            }
        }
        */
    }
}
