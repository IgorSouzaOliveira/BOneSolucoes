﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BOneSolucoes.Resources {
    using System;
    
    
    /// <summary>
    ///   Uma classe de recurso de tipo de alta segurança, para pesquisar cadeias de caracteres localizadas etc.
    /// </summary>
    // Essa classe foi gerada automaticamente pela classe StronglyTypedResourceBuilder
    // através de uma ferramenta como ResGen ou Visual Studio.
    // Para adicionar ou remover um associado, edite o arquivo .ResX e execute ResGen novamente
    // com a opção /str, ou recrie o projeto do VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Retorna a instância de ResourceManager armazenada em cache usada por essa classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BOneSolucoes.Resources.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Substitui a propriedade CurrentUICulture do thread atual para todas as
        ///   pesquisas de recursos que usam essa classe de recurso de tipo de alta segurança.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a  EXEC [BONE_ExecAprov] @UserAprov  = &apos;{0}&apos;
        ///.
        /// </summary>
        internal static string BONE_ExecAprov {
            get {
                return ResourceManager.GetString("BONE_ExecAprov", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT T0.&quot;U_emitCNPJ&quot;, T0.&quot;U_enderEmitIE&quot;, T0.&quot;U_emitxNome&quot;, T0.&quot;U_prodcProd&quot; FROM [@BONEXMLDATA]	T0.
        /// </summary>
        internal static string CarregarXmlImp {
            get {
                return ResourceManager.GetString("CarregarXmlImp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT 
        ///		&apos;&apos; [Checked]
        ///	   ,CardCode [CardCode]
        ///	   ,CardName [CardName]
        ///	   ,GRP.GroupName [GroupName]
        ///	   ,Phone1 [Phone1]
        ///	   ,E_Mail [E_Mail]
        ///	   ,(SELECT TOP 1 CONCAT(A.AddrType,&apos; &apos;,A.Street,&apos;, &apos;,A.StreetNo,&apos; - &apos;,A.Block,&apos;, &apos;, B.Name,&apos; - &apos;,A.State) FROM CRD1 A JOIN OCNT B ON B.AbsId = A.County WHERE A.CardCode = OCRD.CardCode) [Endereço] 
        ///	   ,CASE WHEN validFor = &apos;Y&apos; THEN &apos;Ativo&apos; WHEN validFor = &apos;N&apos; THEN &apos;Inativo&apos; END AS [Situacao]
        ///	   
        ///	   FROM OCRD WITH(NOLOCK)
        ///	   LEFT JOIN OCRG GRP WITH [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string LoadBP {
            get {
                return ResourceManager.GetString("LoadBP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a BEGIN 
        ///	 DECLARE @CardCode AS NVARCHAR(54) = &apos;{0}&apos;	
        ///	 DECLARE @Grupo AS NVARCHAR(54) = &apos;{1}&apos;
        ///	 DECLARE @Situacao AS NVARCHAR(54) = &apos;{2}&apos;	
        ///
        ///SELECT 
        ///       &apos;&apos; [Checked]
        ///	   ,CardCode [CardCode]
        ///	   ,CardName [CardName]
        ///	   ,GRP.GroupName [GroupName]
        ///	   ,Phone1 [Phone1]
        ///	   ,E_Mail [E_Mail]
        ///	   ,(SELECT TOP 1 CONCAT(A.AddrType,&apos; &apos;,A.Street,&apos;, &apos;,A.StreetNo,&apos; - &apos;,A.Block,&apos;, &apos;, B.Name,&apos; - &apos;,A.State) FROM CRD1 A JOIN OCNT B ON B.AbsId = A.County WHERE A.CardCode = OCRD.CardCode) [Endereço] 
        ///	   ,CASE [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string LoadBPFilter {
            get {
                return ResourceManager.GetString("LoadBPFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT &apos;&apos; as &apos;Sel&apos;,T0.&quot;Code&quot;,T0.&quot;U_BONE_ObjectType&quot;, T0.&quot;U_BONE_NomeConsulta&quot;, T0.&quot;U_BONE_Query&quot;,T0.&quot;U_BONE_CodeEtapa&quot;, T0.&quot;U_BOne_EtapaAut&quot;, T0.&quot;U_BOne_Ativo&quot; FROM [@BONMODAPROV] T0.
        /// </summary>
        internal static string LoadConfAprov {
            get {
                return ResourceManager.GetString("LoadConfAprov", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT Code,U_Item,U_Campo, U_Obs,U_Ativo,U_Msg FROM [@CONFBP]
        ///.
        /// </summary>
        internal static string LoadConfig {
            get {
                return ResourceManager.GetString("LoadConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a BEGIN 
        ///	
        ///	DECLARE @DocEntry AS NVARCHAR(254) = &apos;{0}&apos;
        ///	DECLARE @TipoDoc AS NVARCHAR(254) = &apos;{1}&apos;
        ///	DECLARE @DataDe AS DATE = &apos;{2}&apos;
        ///	DECLARE @DataAte AS DATE = &apos;{3}&apos;
        ///	DECLARE @SlpCode AS NVARCHAR(254) = &apos;{4}&apos;
        ///	DECLARE @Filial AS NVARCHAR(254) = &apos;{5}&apos;
        ///	DECLARE @UserAprove AS INT = &apos;{6}&apos;
        ///
        ///SELECT   
        ///  &apos;&apos; as &apos;Sel&apos;,  
        ///  T0.U_BOneDocDate [DocDate],
        ///  CASE WHEN T0.U_BOneTipoDoc = &apos;17&apos; THEN &apos;Pedido de venda&apos;  
        ///  WHEN T0.U_BOneTipoDoc = &apos;540000006&apos; THEN &apos;Oferta de compra&apos;  
        ///  WHEN T0.U_BOneTipoDoc = &apos;22&apos; [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string LoadDocAprove {
            get {
                return ResourceManager.GetString("LoadDocAprove", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a SELECT 
        ///		&apos;&apos;  [Selecionar],
        ///		T0.&quot;DocEntry&quot; [Nº Pedido],
        ///		T0.&quot;CardCode&quot; [Cliente],
        ///		T0.&quot;CardName&quot; [Nome],
        ///		(SELECT C.&quot;Name&quot; FROM OCPR C WHERE C.&quot;CntctCode&quot; = T0.&quot;CntctCode&quot; AND C.&quot;CardCode&quot; = T0.&quot;CardCode&quot;) [Pessoa de contato],
        ///		T0.&quot;BPLName&quot; [Filial], 
        ///		T0.&quot;DocDate&quot; [Data de lançamento], 
        ///		(SELECT A.&quot;SlpName&quot; FROM OSLP A WHERE A.&quot;SlpCode&quot; = T0.&quot;SlpCode&quot;) [Vendedor], 
        ///		(SELECT b.&quot;PymntGroup&quot; FROM OCTG B WHERE B.&quot;GroupNum&quot; = T0.&quot;GroupNum&quot;) [Condição de Pagamento],
        ///		T0.&quot;PeyMethod&quot; [Forma de p [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string LoadPed {
            get {
                return ResourceManager.GetString("LoadPed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a BEGIN
        ///
        ///DECLARE @CardCode AS NVARCHAR(254) = &apos;{0}&apos;
        ///DECLARE @Vendedor AS NVARCHAR(254) = &apos;{1}&apos;
        ///DECLARE @DataDe AS DATE = &apos;{2}&apos;
        ///DECLARE @DataAte AS DATE = &apos;{3}&apos;
        ///DECLARE @NumDocDe AS NVARCHAR(50) = &apos;{4}&apos;
        ///DECLARE @NumDocAte AS NVARCHAR(50) = &apos;{5}&apos;
        ///DECLARE @Filial AS NVARCHAR(254) = &apos;{6}&apos;
        ///
        ///SELECT 
        ///	    &apos;&apos;  [Selecionar],
        ///		T0.&quot;DocEntry&quot; [Nº Pedido],
        ///		T0.&quot;CardCode&quot; [Cliente],
        ///		T0.&quot;CardName&quot; [Nome],
        ///		(SELECT C.&quot;Name&quot; FROM OCPR C WHERE C.&quot;CntctCode&quot; = T0.&quot;CntctCode&quot; AND C.&quot;CardCode&quot; = T0.&quot;CardCode&quot;) [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string LoadPedFilter {
            get {
                return ResourceManager.GetString("LoadPedFilter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;Application&gt;
        ///	&lt;Menus&gt;
        ///		&lt;action type=&quot;add&quot;&gt;
        ///			&lt;Menu Checked=&quot;0&quot; Enabled=&quot;1&quot; FatherUID=&quot;43520&quot; Position=&quot;-1&quot; String=&quot;BOne Soluções&quot; Type=&quot;2&quot; UniqueID=&quot;mnu_mainmenu&quot; Image=&quot;%path%\Imagens\logo_menu.bmp&quot;&gt;
        ///				&lt;Menus&gt;
        ///					&lt;action type=&quot;add&quot;&gt;
        ///						&lt;Menu Checked=&quot;0&quot; Enabled=&quot;1&quot; FatherUID=&quot;mnu_mainmenu&quot; Position=&quot;2&quot; String=&quot;Parametrização&quot; Type=&quot;1&quot; UniqueID=&quot;mnu_mnuParam&quot; /&gt;
        ///					&lt;/action&gt;
        ///				&lt;/Menus&gt;
        ///			&lt;/Menu&gt;
        ///		&lt;/action&gt;
        ///		&lt;action type=&quot;add&quot;&gt;
        ///			&lt;Menu Ch [o restante da cadeia de caracteres foi truncado]&quot;;.
        /// </summary>
        internal static string menuAdd {
            get {
                return ResourceManager.GetString("menuAdd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;Application&gt;
        ///	&lt;Menus&gt;
        ///		&lt;action type=&quot;remove&quot;&gt;
        ///			&lt;Menu UniqueID=&quot;mnu_mainmenu&quot;&gt;&lt;/Menu&gt;
        ///		&lt;/action&gt;
        ///	&lt;/Menus&gt;
        ///&lt;/Application&gt;.
        /// </summary>
        internal static string menuRemove {
            get {
                return ResourceManager.GetString("menuRemove", resourceCulture);
            }
        }
    }
}
