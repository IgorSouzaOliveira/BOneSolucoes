BEGIN 
	 DECLARE @CardCode AS NVARCHAR(54) = '{0}'	
	 DECLARE @Grupo AS NVARCHAR(54) = '{1}'
	 DECLARE @Situacao AS NVARCHAR(54) = '{2}'	

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

	   WHERE ((@CardCode = '') OR (OCRD."CardCode" = @CardCode)) AND
	   ((@Grupo = '') OR (GRP."GroupName" = @Grupo)) AND
	   ((@Situacao = '') OR (OCRD."validFor" = @Situacao))
END