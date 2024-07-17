DECLARE @DocEntry AS NVARCHAR(50) = '{0}'

SELECT 
	T0."ObjType",
	T0."DocEntry",
	T0."CardCode", 
	T0."CardName", 
	T0."BPLId",
	T0."Comments", 
	T0."GroupNum", 
	T0."PeyMethod", 
	T0.SlpCode,
	T1."LineNum", 
	T1."ItemCode", 
	T1."Quantity",
	T1."Price", 
	T1."Usage"
		
FROM [ORDR] T0 
JOIN [RDR1] T1 ON T1.DocEntry = T0.DocEntry
WHERE T0."DocEntry" = @DocEntry ORDER BY T1."LineNum" ASC