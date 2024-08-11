BEGIN 
	
	DECLARE @DocEntry AS NVARCHAR(254) = '{0}'
	DECLARE @TipoDoc AS NVARCHAR(254) = '{1}'
	DECLARE @DataDe AS DATE = '{2}'
	DECLARE @DataAte AS DATE = '{3}'
	DECLARE @SlpCode AS NVARCHAR(254) = '{4}'
	DECLARE @Filial AS NVARCHAR(254) = '{5}'
	DECLARE @UserAprove AS INT = '{6}'

SELECT   
  '' as 'Sel',  
  T0.U_BOneDocDate [DocDate],
  CASE WHEN T0.U_BOneTipoDoc = '17' THEN 'Pedido de venda'  
  WHEN T0.U_BOneTipoDoc = '540000006' THEN 'Oferta de compra'  
  WHEN T0.U_BOneTipoDoc = '22' THEN 'Pedido de compra' END AS [TipoDoc],  
  T0.U_BOneNumDoc [DocEntry],  
  T0.U_BOneCardCode [CardCode],  
  T0.U_BOneCardName [CardName],   
  T0.U_BOneBplName [BplName],  
  (SELECT a.SlpName FROM OSLP A WHERE A.SlpCode = T0.U_BOneSlpCode) [SlpName],  
  T0.U_BOneNameEtapa [NameEtapa],  
  T2.UserID [UserAprove],  
  T0.U_BOneModeloAut [ModeloAut],   
  (SELECT b.PymntGroup FROM OCTG B WHERE B.GroupNum = T0.U_BOnePaymentCode) [PaymentName],  
  T0.U_BOnePaymentMethod [PaymentMethod],   
  T0.U_BOneDocTotal [DocTotal],
  '' AS [Status]
  
FROM [@BONEAPROV] T0 WITH(NOLOCK)  
JOIN OWST T1 WITH(NOLOCK) ON T1.WstCode = T0.U_BOneCodEtapa  
JOIN WST1 T2 WITH(NOLOCK) ON T2.WstCode = T1.WstCode  

WHERE (T0.U_BOneAutorizado = 'FALSE' AND T0.U_BOneProcessado = 0)  
AND ((@DocEntry = '') OR (T0."U_BOneNumDoc" = @DocEntry))
AND ((@DataDe = '') OR (T0."U_BOneDocDate" BETWEEN @DataDe AND @DataAte))
AND ((@TipoDoc = '') OR (T0."U_BOneTipoDoc" = @TipoDoc))
AND ((@SlpCode = '') OR (T0."U_BOneSlpCode" = @SlpCode))
AND ((@Filial = '') OR (T0."U_BOneBplID" = @Filial))
AND (T2."UserID" = @UserAprove)

END