﻿SELECT '' as 'Sel',T0."Code",T0."U_BONE_ObjectType", T0."U_BONE_NomeConsulta", T0."U_BONE_Query",T0."U_BONE_CodeEtapa", T0."U_BOne_EtapaAut", T0."U_BOne_Ativo" 

FROM [@BONMODAPROV] T0 
ORDER BY CAST(T0."Code" AS INT) ASC