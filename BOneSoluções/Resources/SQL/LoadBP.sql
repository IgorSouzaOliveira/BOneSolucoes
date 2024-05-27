SELECT 
		'' [Checked]
	   ,CardCode [CardCode]
	   ,CardName [CardName]
	   ,GRP.GroupName [GroupName]
	   ,Phone1 [Phone1]
	   ,E_Mail [E_Mail]
	   ,(SELECT TOP 1 CONCAT(A.AddrType,' ',A.Street,', ',A.StreetNo,' - ',A.Block,', ', B.Name,' - ',A.State) FROM CRD1 A JOIN OCNT B ON B.AbsId = A.County WHERE A.CardCode = OCRD.CardCode) [Endereço] 
	   ,CASE WHEN validFor = 'Y' THEN 'Ativo' WHEN validFor = 'N' THEN 'Inativo' END AS [Situacao]
	   
	   FROM OCRD WITH(NOLOCK)
	   LEFT JOIN OCRG GRP WITH(NOLOCK) ON GRP.GroupCode = OCRD.GroupCode
