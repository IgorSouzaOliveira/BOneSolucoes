DECLARE @ChaveAcesso AS NVARCHAR(MAX) = '{0}'


SELECT

   	'' AS [Check],
	'CardCode' = (SELECT TOP 1 A.CardCode FROM CRD7 A WHERE A."TaxId0" = T0.U_emitCNPJ AND A.CardCode = (SELECT B.CardCode FROM OSCN B )),
	'CardName' = (SELECT B.CardName FROM OCRD B WHERE B.CardCode = (SELECT TOP 1 A.CardCode FROM CRD7 A WHERE A."TaxId0" = T0.U_emitCNPJ) AND B.CardCode = (SELECT C.CardCode FROM OSCN C)),
    'CNPJ' = T0."U_emitCNPJ", 
	'IE' = T0."U_enderEmitIE",
	'ItemCode' = (SELECT C.ItemCode FROM OSCN C WHERE C.CardCode = (SELECT TOP 1 A.CardCode FROM CRD7 A WHERE A."TaxId0" = T0.U_emitCNPJ)),
	'ItemName' = (SELECT C.ItemName FROM OITM C WHERE C.ItemCode = (SELECT C.ItemCode FROM OSCN C WHERE C.CardCode = (SELECT TOP 1 A.CardCode FROM CRD7 A WHERE A."TaxId0" = T0.U_emitCNPJ))),
	'EAN' = T0.U_prodcEAN,
	'Quantidade' = T0.U_prodqCom,
	'Preço' = T0.U_prodvUnCom,
	'' AS [Usage]
FROM [@BONEXMLDATA]	T0
WHERE T0."U_ChaveAcesso" = @ChaveAcesso
