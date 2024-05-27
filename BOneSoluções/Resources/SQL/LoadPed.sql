SELECT 
		''  [Selecionar],
		T0."DocEntry" [Nº Pedido],
		T0."CardCode" [Cliente],
		T0."CardName" [Nome],
		(SELECT C."Name" FROM OCPR C WHERE C."CntctCode" = T0."CntctCode" AND C."CardCode" = T0."CardCode") [Pessoa de contato],
		T0."BPLName" [Filial], 
		T0."DocDate" [Data de lançamento], 
		(SELECT A."SlpName" FROM OSLP A WHERE A."SlpCode" = T0."SlpCode") [Vendedor], 
		(SELECT b."PymntGroup" FROM OCTG B WHERE B."GroupNum" = T0."GroupNum") [Condição de Pagamento],
		T0."PeyMethod" [Forma de pagamento], 
		T0."DocTotal" [Total do documento], 
		T0."Comments" [Observações]

FROM ORDR T0
WHERE T0."DocStatus" = 'O'
AND T0."Canceled" = 'N'