// ---------------------------------------------------------
// Lutz Roeder's .NET Reflector
// Copyright (c) 2000-2007 Lutz Roeder. All rights reserved.
// ---------------------------------------------------------
namespace Reflector.CodeModel
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using Reflector.CodeModel;
    using Reflector.CodeModel.Memory;

    public class Visitor
    {
        public virtual void VisitAssembly(IAssembly value)
        {
            this.VisitCustomAttributeCollection(value.Attributes);
            this.VisitModuleCollection(value.Modules);
        }

        public virtual void VisitAssemblyReference(IAssemblyReference value)
        {
        }

        public virtual void VisitModule(IModule value)
        {
            this.VisitCustomAttributeCollection(value.Attributes);
        }

        public virtual void VisitModuleReference(IModuleReference value)
        {
        }

        public virtual void VisitNamespace(INamespace value)
        {
            this.VisitTypeDeclarationCollection(value.Types);
        }

        public virtual void VisitTypeDeclaration(ITypeDeclaration value)
        {
            this.VisitCustomAttributeCollection(value.Attributes);
            this.VisitMethodDeclarationCollection(value.Methods);
            this.VisitFieldDeclarationCollection(value.Fields);
            this.VisitPropertyDeclarationCollection(value.Properties);
            this.VisitEventDeclarationCollection(value.Events);
            this.VisitTypeDeclarationCollection(value.NestedTypes);
        }

        public virtual void VisitTypeReference(ITypeReference value)
        {
            this.VisitTypeCollection(value.GenericArguments);
        }

        public virtual void VisitMethodDeclaration(IMethodDeclaration value)
        {
            this.VisitCustomAttributeCollection(value.Attributes);
            this.VisitParameterDeclarationCollection(value.Parameters);
            this.VisitMethodReferenceCollection(value.Overrides);
            this.VisitMethodReturnType(value.ReturnType);

            IBlockStatement body = value.Body as IBlockStatement;
            if (body != null)
            {
                this.VisitStatement(body);
            }

            //	IMethodBody methodBody = value.Body as IMethodBody;
            //	if (methodBody != null)
            //	{
            //		this.VisitTypeCollection(methodBody.LocalVariables);
            //		this.VisitExceptionHandlerCollection(methodBody.ExceptionHandlers);
            //		this.VisitInstructionCollection(methodBody.Instructions);
            //	}
        }

        public virtual void VisitFieldDeclaration(IFieldDeclaration value)
        {
            this.VisitCustomAttributeCollection(value.Attributes);
            this.VisitType(value.FieldType);
        }

        public virtual void VisitPropertyDeclaration(IPropertyDeclaration value)
        {
            this.VisitCustomAttributeCollection(value.Attributes);
            this.VisitType(value.PropertyType);
        }

        public virtual void VisitEventDeclaration(IEventDeclaration value)
        {
            this.VisitCustomAttributeCollection(value.Attributes);
            this.VisitType(value.EventType);
        }

        public virtual void VisitMethodReturnType(IMethodReturnType value)
        {
            this.VisitCustomAttributeCollection(value.Attributes);
            this.VisitType(value.Type);
        }

        public virtual void VisitParameterDeclaration(IParameterDeclaration value)
        {
            this.VisitCustomAttributeCollection(value.Attributes);
            this.VisitType(value.ParameterType);
        }

        public virtual void VisitResource(IResource value)
        {
            IEmbeddedResource embeddedResource = value as IEmbeddedResource;
            if (embeddedResource != null)
            {
                this.VisitEmbeddedResource(embeddedResource);
            }

            IFileResource fileResource = value as IFileResource;
            if (fileResource != null)
            {
                this.VisitFileResource(fileResource);
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid resource type '{0}'.", value.GetType().Name));
        }

        public virtual void VisitEmbeddedResource(IEmbeddedResource value)
        {
        }

        public virtual void VisitFileResource(IFileResource value)
        {
        }

        public virtual void VisitType(IType value)
        {
            if (value == null)
            {
                return;
            }

            ITypeReference typeReference = value as ITypeReference;
            if (typeReference != null)
            {
                this.VisitTypeReference(typeReference);
                return;
            }

            IArrayType arrayType = value as IArrayType;
            if (arrayType != null)
            {
                this.VisitArrayType(arrayType);
                return;
            }

            IPointerType pointerType = value as IPointerType;
            if (pointerType != null)
            {
                this.VisitPointerType(pointerType);
                return;
            }

            IReferenceType referenceType = value as IReferenceType;
            if (referenceType != null)
            {
                this.VisitReferenceType(referenceType);
                return;
            }

            IOptionalModifier optionalModifier = value as IOptionalModifier;
            if (optionalModifier != null)
            {
                this.VisitOptionalModifier(optionalModifier);
                return;
            }

            IRequiredModifier requiredModifier = value as IRequiredModifier;
            if (requiredModifier != null)
            {
                this.VisitRequiredModifier(requiredModifier);
                return;
            }

            IFunctionPointer functionPointer = value as IFunctionPointer;
            if (functionPointer != null)
            {
                this.VisitFunctionPointer(functionPointer);
                return;
            }

            IGenericParameter genericParameter = value as IGenericParameter;
            if (genericParameter != null)
            {
                this.VisitGenericParameter(genericParameter);
                return;
            }

            IGenericArgument genericArgument = value as IGenericArgument;
            if (genericArgument != null)
            {
                this.VisitGenericArgument(genericArgument);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid type '{0}'.", value.GetType().Name));
        }

        public virtual void VisitArrayType(IArrayType value)
        {
            this.VisitType(value.ElementType);
            this.VisitArrayDimensionCollection(value.Dimensions);
        }

        public virtual void VisitPointerType(IPointerType value)
        {
            this.VisitType(value.ElementType);
        }

        public virtual void VisitReferenceType(IReferenceType value)
        {
            this.VisitType(value.ElementType);
        }

        public virtual void VisitOptionalModifier(IOptionalModifier type)
        {
            this.VisitType(type.Modifier);
            this.VisitType(type.ElementType);
        }

        public virtual void VisitRequiredModifier(IRequiredModifier type)
        {
            this.VisitType(type.Modifier);
            this.VisitType(type.ElementType);
        }

        public virtual void VisitFunctionPointer(IFunctionPointer type)
        {
        }

        public virtual void VisitGenericParameter(IGenericParameter type)
        {
        }

        public virtual void VisitGenericArgument(IGenericArgument type)
        {
        }

        public virtual void VisitCustomAttribute(ICustomAttribute customAttribute)
        {
            this.VisitExpressionCollection(customAttribute.Arguments);
        }

        public virtual void VisitStatement(IStatement value)
        {
            if (value == null)
            {
                return;
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
                this.VisitExpressionStatement(value as IExpressionStatement);
                return;
            }

            if (value is IBlockStatement)
            {
                this.VisitBlockStatement(value as IBlockStatement);
                return;
            }

            if (value is IConditionStatement)
            {
                this.VisitConditionStatement(value as IConditionStatement);
                return;
            }

            if (value is IMethodReturnStatement)
            {
                this.VisitMethodReturnStatement(value as IMethodReturnStatement);
                return;
            }

            if (value is ILabeledStatement)
            {
                this.VisitLabeledStatement(value as ILabeledStatement);
                return;
            }

            if (value is IGotoStatement)
            {
                this.VisitGotoStatement(value as IGotoStatement);
                return;
            }

            if (value is IForStatement)
            {
                this.VisitForStatement(value as IForStatement);
                return;
            }

            if (value is IForEachStatement)
            {
                this.VisitForEachStatement(value as IForEachStatement);
                return;
            }

            if (value is IWhileStatement)
            {
                this.VisitWhileStatement(value as IWhileStatement);
                return;
            }

            if (value is IDoStatement)
            {
                this.VisitDoStatement(value as IDoStatement);
                return;
            }

            if (value is ITryCatchFinallyStatement)
            {
                this.VisitTryCatchFinallyStatement(value as ITryCatchFinallyStatement);
                return;
            }

            if (value is IThrowExceptionStatement)
            {
                this.VisitThrowExceptionStatement(value as IThrowExceptionStatement);
                return;
            }

            if (value is IAttachEventStatement)
            {
                this.VisitAttachEventStatement(value as IAttachEventStatement);
                return;
            }

            if (value is IRemoveEventStatement)
            {
                this.VisitRemoveEventStatement(value as IRemoveEventStatement);
                return;
            }

            if (value is ISwitchStatement)
            {
                this.VisitSwitchStatement(value as ISwitchStatement);
                return;
            }

            if (value is IBreakStatement)
            {
                this.VisitBreakStatement(value as IBreakStatement);
                return;
            }

            if (value is IContinueStatement)
            {
                this.VisitContinueStatement(value as IContinueStatement);
                return;
            }

            if (value is ICommentStatement)
            {
                this.VisitCommentStatement(value as ICommentStatement);
                return;
            }

            if (value is IUsingStatement)
            {
                this.VisitUsingStatement(value as IUsingStatement);
                return;
            }

            if (value is IFixedStatement)
            {
                this.VisitFixedStatement(value as IFixedStatement);
                return;
            }

            if (value is ILockStatement)
            {
                this.VisitLockStatement(value as ILockStatement);
                return;
            }

            if (value is IMemoryCopyStatement)
            {
                this.VisitMemoryCopyStatement(value as IMemoryCopyStatement);
                return;
            }

            if (value is IMemoryInitializeStatement)
            {
                this.VisitMemoryInitializeStatement(value as IMemoryInitializeStatement);
                return;
            }

            if (value is IDebugBreakStatement)
            {
                this.VisitDebugBreakStatement(value as IDebugBreakStatement);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid statement type '{0}'.", value.GetType().Name));
        }

        public virtual void VisitBlockStatement(IBlockStatement value)
        {
            this.VisitStatementCollection(value.Statements);
        }

        public virtual void VisitCommentStatement(ICommentStatement value)
        {
        }

        public virtual void VisitMethodReturnStatement(IMethodReturnStatement value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitMemoryCopyStatement(IMemoryCopyStatement value)
        {
            this.VisitExpression(value.Source);
            this.VisitExpression(value.Destination);
            this.VisitExpression(value.Length);
        }

        public virtual void VisitMemoryInitializeStatement(IMemoryInitializeStatement value)
        {
            this.VisitExpression(value.Offset);
            this.VisitExpression(value.Value);
            this.VisitExpression(value.Length);
        }

        public virtual void VisitDebugBreakStatement(IDebugBreakStatement value)
        {
        }

        public virtual void VisitConditionStatement(IConditionStatement value)
        {
            this.VisitExpression(value.Condition);
            this.VisitStatement(value.Then);
            this.VisitStatement(value.Else);
        }

        public virtual void VisitTryCatchFinallyStatement(ITryCatchFinallyStatement value)
        {
            this.VisitStatement(value.Try);
            this.VisitCatchClauseCollection(value.CatchClauses);
            this.VisitStatement(value.Finally);
            this.VisitStatement(value.Fault);
        }

        public virtual void VisitAssignExpression(IAssignExpression value)
        {
            this.VisitExpression(value.Target);
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitExpressionStatement(IExpressionStatement value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitForStatement(IForStatement value)
        {
            this.VisitStatement(value.Initializer);
            this.VisitExpression(value.Condition);
            this.VisitStatement(value.Increment);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitForEachStatement(IForEachStatement value)
        {
            this.VisitVariableDeclaration(value.Variable);
            this.VisitExpression(value.Expression);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitUsingStatement(IUsingStatement value)
        {
            this.VisitExpression(value.Expression);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitFixedStatement(IFixedStatement value)
        {
            this.VisitVariableDeclaration(value.Variable);
            this.VisitExpression(value.Expression);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitLockStatement(ILockStatement value)
        {
            this.VisitExpression(value.Expression);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitWhileStatement(IWhileStatement value)
        {
            this.VisitExpression(value.Condition);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitDoStatement(IDoStatement value)
        {
            this.VisitExpression(value.Condition);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitBreakStatement(IBreakStatement value)
        {
        }

        public virtual void VisitContinueStatement(IContinueStatement value)
        {
        }
        public virtual void VisitThrowExceptionStatement(IThrowExceptionStatement value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitAttachEventStatement(IAttachEventStatement value)
        {
            this.VisitExpression(value.Event);
            this.VisitExpression(value.Listener);
        }

        public virtual void VisitRemoveEventStatement(IRemoveEventStatement value)
        {
            this.VisitExpression(value.Event);
            this.VisitExpression(value.Listener);
        }

        public virtual void VisitSwitchStatement(ISwitchStatement value)
        {
            this.VisitExpression(value.Expression);
            this.VisitSwitchCaseCollection(value.Cases);
        }

        public virtual void VisitGotoStatement(IGotoStatement value)
        {
        }

        public virtual void VisitLabeledStatement(ILabeledStatement value)
        {
            this.VisitStatement(value.Statement);
        }

        public virtual void VisitExpression(IExpression value)
        {
            if (value == null)
            {
                return;
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
                this.VisitVariableReferenceExpression(value as IVariableReferenceExpression);
                return;
            }

            if (value is ILiteralExpression)
            {
                this.VisitLiteralExpression(value as ILiteralExpression);
                return;
            }

            if (value is IFieldReferenceExpression)
            {
                this.VisitFieldReferenceExpression(value as IFieldReferenceExpression);
                return;
            }

            if (value is IPropertyReferenceExpression)
            {
                this.VisitPropertyReferenceExpression(value as IPropertyReferenceExpression);
                return;
            }

            if (value is IAssignExpression)
            {
                this.VisitAssignExpression(value as IAssignExpression);
                return;
            }

            if (value is IBinaryExpression)
            {
                this.VisitBinaryExpression(value as IBinaryExpression);
                return;
            }

            if (value is IThisReferenceExpression)
            {
                this.VisitThisReferenceExpression(value as IThisReferenceExpression);
                return;
            }

            if (value is IMethodInvokeExpression)
            {
                this.VisitMethodInvokeExpression(value as IMethodInvokeExpression);
                return;
            }

            if (value is IMethodReferenceExpression)
            {
                this.VisitMethodReferenceExpression(value as IMethodReferenceExpression);
                return;
            }

            if (value is IArgumentReferenceExpression)
            {
                this.VisitArgumentReferenceExpression(value as IArgumentReferenceExpression);
                return;
            }

            if (value is IVariableDeclarationExpression)
            {
                this.VisitVariableDeclarationExpression(value as IVariableDeclarationExpression);
                return;
            }

            if (value is ITypeReferenceExpression)
            {
                this.VisitTypeReferenceExpression(value as ITypeReferenceExpression);
                return;
            }

            if (value is IBaseReferenceExpression)
            {
                this.VisitBaseReferenceExpression(value as IBaseReferenceExpression);
                return;
            }

            if (value is IUnaryExpression)
            {
                this.VisitUnaryExpression(value as IUnaryExpression);
                return;
            }

            if (value is ITryCastExpression)
            {
                this.VisitTryCastExpression(value as ITryCastExpression);
                return;
            }

            if (value is ICanCastExpression)
            {
                this.VisitCanCastExpression(value as ICanCastExpression);
                return;
            }

            if (value is ICastExpression)
            {
                this.VisitCastExpression(value as ICastExpression);
                return;
            }

            if (value is ITypeOfExpression)
            {
                this.VisitTypeOfExpression(value as ITypeOfExpression);
                return;
            }

            if (value is IFieldOfExpression)
            {
                this.VisitFieldOfExpression(value as IFieldOfExpression);
                return;
            }

            if (value is IMethodOfExpression)
            {
                this.VisitMethodOfExpression(value as IMethodOfExpression);
                return;
            }

            if (value is IMemberInitializerExpression)
            {
                this.VisitMemberInitializerExpression(value as IMemberInitializerExpression);
                return;
            }

            if (value is IEventReferenceExpression)
            {
                this.VisitEventReferenceExpression(value as IEventReferenceExpression);
                return;
            }

            if (value is IArgumentListExpression)
            {
                this.VisitArgumentListExpression(value as IArgumentListExpression);
                return;
            }

            if (value is IArrayCreateExpression)
            {
                this.VisitArrayCreateExpression(value as IArrayCreateExpression);
                return;
            }

            if (value is IBlockExpression)
            {
                this.VisitBlockExpression(value as IBlockExpression);
                return;
            }

            if (value is IConditionExpression)
            {
                this.VisitConditionExpression(value as IConditionExpression);
                return;
            }

            if (value is INullCoalescingExpression)
            {
                this.VisitNullCoalescingExpression(value as INullCoalescingExpression);
                return;
            }

            if (value is IDelegateCreateExpression)
            {
                this.VisitDelegateCreateExpression(value as IDelegateCreateExpression);
                return;
            }

            if (value is IAnonymousMethodExpression)
            {
                this.VisitAnonymousMethodExpression(value as IAnonymousMethodExpression);
                return;
            }

            if (value is IPropertyIndexerExpression)
            {
                this.VisitPropertyIndexerExpression(value as IPropertyIndexerExpression);
                return;
            }

            if (value is IArrayIndexerExpression)
            {
                this.VisitArrayIndexerExpression(value as IArrayIndexerExpression);
                return;
            }

            if (value is IDelegateInvokeExpression)
            {
                this.VisitDelegateInvokeExpression(value as IDelegateInvokeExpression);
                return;
            }

            if (value is IObjectCreateExpression)
            {
                this.VisitObjectCreateExpression(value as IObjectCreateExpression);
                return;
            }

            if (value is IAddressOfExpression)
            {
                this.VisitAddressOfExpression(value as IAddressOfExpression);
                return;
            }

            if (value is IAddressReferenceExpression)
            {
                this.VisitAddressReferenceExpression(value as IAddressReferenceExpression);
                return;
            }

            if (value is IAddressOutExpression)
            {
                this.VisitAddressOutExpression(value as IAddressOutExpression);
                return;
            }

            if (value is IAddressDereferenceExpression)
            {
                this.VisitAddressDereferenceExpression(value as IAddressDereferenceExpression);
                return;
            }

            if (value is ISizeOfExpression)
            {
                this.VisitSizeOfExpression(value as ISizeOfExpression);
                return;
            }

            if (value is ITypedReferenceCreateExpression)
            {
                this.VisitTypedReferenceCreateExpression(value as ITypedReferenceCreateExpression);
                return;
            }

            if (value is ITypeOfTypedReferenceExpression)
            {
                this.VisitTypeOfTypedReferenceExpression(value as ITypeOfTypedReferenceExpression);
                return;
            }

            if (value is IValueOfTypedReferenceExpression)
            {
                this.VisitValueOfTypedReferenceExpression(value as IValueOfTypedReferenceExpression);
                return;
            }

            if (value is IStackAllocateExpression)
            {
                this.VisitStackAllocateExpression(value as IStackAllocateExpression);
                return;
            }

            if (value is IGenericDefaultExpression)
            {
                this.VisitGenericDefaultExpression(value as IGenericDefaultExpression);
                return;
            }

            if (value is IQueryExpression)
            {
                this.VisitQueryExpression(value as IQueryExpression);
                return;
            }

            if (value is ILambdaExpression)
            {
                this.VisitLambdaExpression(value as ILambdaExpression);
                return;
            }

            if (value is ISnippetExpression)
            {
                this.VisitSnippetExpression(value as ISnippetExpression);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid expression type '{0}'.", value.GetType().Name));
        }

        public virtual void VisitVariableDeclarationExpression(IVariableDeclarationExpression value)
        {
            this.VisitVariableDeclaration(value.Variable);
        }

        public virtual void VisitMemberInitializerExpression(IMemberInitializerExpression value)
        {
            this.VisitExpression(value.Value);
        }

        public virtual void VisitTypeOfExpression(ITypeOfExpression value)
        {
            this.VisitType(value.Type);
        }

        public virtual void VisitFieldOfExpression(IFieldOfExpression value)
        {
            this.VisitFieldReference(value.Field);
        }

        public virtual void VisitMethodOfExpression(IMethodOfExpression value)
        {
            this.VisitMethodReference(value.Method);
            if (value.Type != null)
            {
                this.VisitTypeReference(value.Type);
            }
        }

        public virtual void VisitArrayCreateExpression(IArrayCreateExpression value)
        {
            this.VisitType(value.Type);
            this.VisitExpression(value.Initializer);
            this.VisitExpressionCollection(value.Dimensions);
        }

        public virtual void VisitBlockExpression(IBlockExpression value)
        {
            this.VisitExpressionCollection(value.Expressions);
        }

        public virtual void VisitBaseReferenceExpression(IBaseReferenceExpression value)
        {
        }

        public virtual void VisitTryCastExpression(ITryCastExpression value)
        {
            this.VisitType(value.TargetType);
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitCanCastExpression(ICanCastExpression value)
        {
            this.VisitType(value.TargetType);
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitCastExpression(ICastExpression value)
        {
            this.VisitType(value.TargetType);
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitConditionExpression(IConditionExpression value)
        {
            this.VisitExpression(value.Condition);
            this.VisitExpression(value.Then);
            this.VisitExpression(value.Else);
        }

        public virtual void VisitNullCoalescingExpression(INullCoalescingExpression value)
        {
            this.VisitExpression(value.Condition);
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitDelegateCreateExpression(IDelegateCreateExpression value)
        {
            this.VisitType(value.DelegateType);
            this.VisitExpression(value.Target);
        }

        public virtual void VisitAnonymousMethodExpression(IAnonymousMethodExpression value)
        {
            this.VisitType(value.DelegateType);
            this.VisitParameterDeclarationCollection(value.Parameters);
            this.VisitMethodReturnType(value.ReturnType);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitQueryExpression(IQueryExpression value)
        {
            this.VisitFromClause(value.From);
            this.VisitQueryBody(value.Body);
        }

        public virtual void VisitQueryBody(IQueryBody value)
        {
            this.VisitQueryOperation(value.Operation);
            this.VisitQueryClauseCollection(value.Clauses);

            if (value.Continuation != null)
            {
                this.VisitQueryContinuation(value.Continuation);
            }
        }

        public virtual void VisitQueryClause(IQueryClause value)
        {
            if (value is IFromClause)
            {
                this.VisitFromClause(value as IFromClause);
                return;
            }

            if (value is IWhereClause)
            {
                this.VisitWhereClause(value as IWhereClause);
                return;
            }

            if (value is ILetClause)
            {
                this.VisitLetClause(value as ILetClause);
                return;
            }

            if (value is IJoinClause)
            {
                this.VisitJoinClause(value as IJoinClause);
                return;
            }

            if (value is IOrderClause)
            {
                this.VisitOrderClause(value as IOrderClause);
                return;
            }

            throw new NotSupportedException();
        }

        public virtual void VisitQueryOperation(IQueryOperation value)
        {
            if (value is ISelectOperation)
            {
                this.VisitSelectOperation(value as ISelectOperation);
                return;
            }

            if (value is IGroupOperation)
            {
                this.VisitGroupOperation(value as IGroupOperation);
                return;
            }

            throw new NotSupportedException();
        }

        public virtual void VisitQueryContinuation(IQueryContinuation value)
        {
            this.VisitVariableDeclaration(value.Variable);
            this.VisitQueryBody(value.Body);
        }

        public virtual void VisitSelectOperation(ISelectOperation value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitGroupOperation(IGroupOperation value)
        {
            this.VisitExpression(value.Item);
            this.VisitExpression(value.Key);
        }

        public virtual void VisitFromClause(IFromClause value)
        {
            this.VisitVariableDeclaration(value.Variable);
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitWhereClause(IWhereClause value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitLetClause(ILetClause value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitJoinClause(IJoinClause value)
        {
            this.VisitVariableDeclaration(value.Variable);
            this.VisitExpression(value.In);
            this.VisitExpression(value.On);
            this.VisitExpression(value.Equality);
            if (value.Into != null)
            {
                this.VisitVariableDeclaration(value.Into);
            }
        }

        public virtual void VisitOrderClause(IOrderClause value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitLambdaExpression(ILambdaExpression value)
        {
            this.VisitVariableDeclarationCollection(value.Parameters);
            this.VisitExpression(value.Body);
        }

        public virtual void VisitTypeReferenceExpression(ITypeReferenceExpression value)
        {
            this.VisitType(value.Type);
        }

        public virtual void VisitFieldReferenceExpression(IFieldReferenceExpression value)
        {
            this.VisitFieldReference(value.Field);
            this.VisitExpression(value.Target);
        }

        public virtual void VisitArgumentReferenceExpression(IArgumentReferenceExpression value)
        {
        }

        public virtual void VisitArgumentListExpression(IArgumentListExpression value)
        {
        }

        public virtual void VisitVariableReferenceExpression(IVariableReferenceExpression value)
        {
            this.VisitVariableReference(value.Variable);
        }

        public virtual void VisitPropertyIndexerExpression(IPropertyIndexerExpression value)
        {
            this.VisitExpressionCollection(value.Indices);
            this.VisitExpression(value.Target);
        }

        public virtual void VisitArrayIndexerExpression(IArrayIndexerExpression value)
        {
            this.VisitExpressionCollection(value.Indices);
            this.VisitExpression(value.Target);
        }

        public virtual void VisitMethodInvokeExpression(IMethodInvokeExpression value)
        {
            this.VisitExpressionCollection(value.Arguments);
            this.VisitExpression(value.Method);
        }

        public virtual void VisitMethodReferenceExpression(IMethodReferenceExpression value)
        {
            this.VisitExpression(value.Target);
        }

        public virtual void VisitEventReferenceExpression(IEventReferenceExpression value)
        {
            this.VisitEventReference(value.Event);
            this.VisitExpression(value.Target);
        }

        public virtual void VisitDelegateInvokeExpression(IDelegateInvokeExpression value)
        {
            this.VisitExpressionCollection(value.Arguments);
            this.VisitExpression(value.Target);
        }

        public virtual void VisitObjectCreateExpression(IObjectCreateExpression value)
        {
            this.VisitType(value.Type);

            if (value.Constructor != null)
            {
                this.VisitMethodReference(value.Constructor);
            }

            this.VisitExpressionCollection(value.Arguments);

            if (value.Initializer != null)
            {
                this.VisitBlockExpression(value.Initializer);
            }
        }

        public virtual void VisitPropertyReferenceExpression(IPropertyReferenceExpression value)
        {
            this.VisitPropertyReference(value.Property);
            this.VisitExpression(value.Target);
        }

        public virtual void VisitThisReferenceExpression(IThisReferenceExpression value)
        {
        }

        public virtual void VisitAddressOfExpression(IAddressOfExpression value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitAddressReferenceExpression(IAddressReferenceExpression value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitAddressOutExpression(IAddressOutExpression value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitAddressDereferenceExpression(IAddressDereferenceExpression value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitSizeOfExpression(ISizeOfExpression value)
        {
            this.VisitType(value.Type);
        }

        public virtual void VisitStackAllocateExpression(IStackAllocateExpression value)
        {
            this.VisitType(value.Type);
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitSnippetExpression(ISnippetExpression value)
        {
        }

        public virtual void VisitUnaryExpression(IUnaryExpression value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitBinaryExpression(IBinaryExpression value)
        {
            this.VisitExpression(value.Left);
            this.VisitExpression(value.Right);
        }

        public virtual void VisitLiteralExpression(ILiteralExpression value)
        {
        }

        public virtual void VisitGenericDefaultExpression(IGenericDefaultExpression value)
        {
        }

        public virtual void VisitTypeOfTypedReferenceExpression(ITypeOfTypedReferenceExpression value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitValueOfTypedReferenceExpression(IValueOfTypedReferenceExpression value)
        {
            this.VisitType(value.TargetType);
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitTypedReferenceCreateExpression(ITypedReferenceCreateExpression value)
        {
            this.VisitExpression(value.Expression);
        }

        public virtual void VisitArrayDimension(IArrayDimension value)
        {
        }

        public virtual void VisitSwitchCase(ISwitchCase value)
        {
            if (value == null)
            {
                return;
            }

            IConditionCase conditionCase = value as IConditionCase;
            if (conditionCase != null)
            {
                this.VisitConditionCase(conditionCase);
                return;
            }

            IDefaultCase defaultCase = value as IDefaultCase;
            if (defaultCase != null)
            {
                this.VisitDefaultCase(defaultCase);
                return;
            }

            throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Invalid switch case type '{0}'.", value.GetType().Name));
        }

        public virtual void VisitDefaultCase(IDefaultCase value)
        {
            this.VisitStatement(value.Body);
        }

        public virtual void VisitConditionCase(IConditionCase value)
        {
            this.VisitExpression(value.Condition);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitCatchClause(ICatchClause value)
        {
            this.VisitVariableDeclaration(value.Variable);
            this.VisitExpression(value.Condition);
            this.VisitStatement(value.Body);
        }

        public virtual void VisitVariableDeclaration(IVariableDeclaration value)
        {
            this.VisitType(value.VariableType);
        }

        public virtual void VisitVariableReference(IVariableReference value)
        {
        }

        public virtual void VisitMethodReference(IMethodReference value)
        {
            this.VisitMethodReturnType(value.ReturnType);
        }

        public virtual void VisitFieldReference(IFieldReference value)
        {
            this.VisitType(value.FieldType);
        }

        public virtual void VisitPropertyReference(IPropertyReference value)
        {
            this.VisitType(value.PropertyType);
        }

        public virtual void VisitEventReference(IEventReference value)
        {
            this.VisitType(value.EventType);
        }

        public virtual void VisitModuleCollection(IModuleCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitModule(value[i]);
            }
        }

        public virtual void VisitResourceCollection(IResourceCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitResource(value[i]);
            }
        }

        public virtual void VisitTypeCollection(ITypeCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitType(value[i]);
            }
        }

        public virtual void VisitTypeDeclarationCollection(ITypeDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitTypeDeclaration(value[i]);
            }
        }

        public virtual void VisitFieldDeclarationCollection(IFieldDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitFieldDeclaration(value[i]);
            }
        }

        public virtual void VisitMethodDeclarationCollection(IMethodDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitMethodDeclaration(value[i]);
            }
        }

        public virtual void VisitPropertyDeclarationCollection(IPropertyDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitPropertyDeclaration(value[i]);
            }
        }

        public virtual void VisitEventDeclarationCollection(IEventDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitEventDeclaration(value[i]);
            }
        }

        public virtual void VisitCustomAttributeCollection(ICustomAttributeCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitCustomAttribute(value[i]);
            }
        }

        public virtual void VisitParameterDeclarationCollection(IParameterDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitParameterDeclaration(value[i]);
            }
        }

        public virtual void VisitVariableDeclarationCollection(IVariableDeclarationCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitVariableDeclaration(value[i]);
            }
        }

        public virtual void VisitMethodReferenceCollection(IMethodReferenceCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitMethodReference(value[i]);
            }
        }

        public virtual void VisitStatementCollection(IStatementCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitStatement(value[i]);
            }
        }

        public virtual void VisitExpressionCollection(IExpressionCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitExpression(value[i]);
            }
        }

        public virtual void VisitQueryClauseCollection(IQueryClauseCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitQueryClause(value[i]);
            }
        }

        public virtual void VisitCatchClauseCollection(ICatchClauseCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitCatchClause(value[i]);
            }
        }

        public virtual void VisitSwitchCaseCollection(ISwitchCaseCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitSwitchCase(value[i]);
            }
        }

        public virtual void VisitArrayDimensionCollection(IArrayDimensionCollection value)
        {
            for (int i = 0; i < value.Count; i++)
            {
                this.VisitArrayDimension(value[i]);
            }
        }
    }
}
