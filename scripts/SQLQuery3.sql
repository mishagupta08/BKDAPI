USE [BKDH]
GO
/****** Object:  StoredProcedure [dbo].[sp_AssignUser]    Script Date: 03/20/2020 23:01:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[sp_AssignUser](@regXML XML)            
as            
BEGIN            
DECLARE @LocalError INT                                                              
 ,@ErrorMessage VARCHAR(4000)                                                   
 Declare @Identity int           
 Declare @Type VARCHAR(500)                                                           
            
            
BEGIN TRY                                                              
  BEGIN TRANSACTION TestTransaction                 
              
              
--      declare @regXML as XML='<AssignData>    
--<Assign>    
--<Id>36</Id>    
--<UserId>91</UserId>    
--<Type>COOK</Type>    
--</Assign>    
--<Assign>    
--<Id>37</Id>    
--<UserId>96</UserId>    
--<Type>COOK</Type>    
--</Assign>    
--</AssignData>    
--'   

	     
              
    Select                                                            
   Assign.query('Id').value('.', 'int') as ID,                                                            
   Assign.query('UserId').value('.', 'int') as UserId,        
   Assign.query('Type').value('.', 'nvarchar(100)') as Type            
   INTO #Assign                                                          
FROM                                
   @regXML.nodes('AssignData/Assign')AS FlightData(Assign)                        
              
 SELECT @Type=Type from #Assign;         
 
 
           
 IF @Type='COOK'        
 BEGIN      
    Insert into tblAssignUserDetail(UserID,OrderID,AssignDate,RectimeStamp)    
     SELECT OrderToKitchen,B.ID,Getdate(),Getdate() FROM trnFoodOrderDetail A INNER JOIN #Assign B ON A.ID=B.ID 
     INNER JOIN trnFoodOrderMain C ON A.OrderID=C.OrderID 
   UPDATE trnFoodOrderDetail set CookID=B.UserId             
   FROM trnFoodOrderDetail A INNER JOIN #Assign B ON A.ID=B.ID
               
   END        
    IF @Type='SV'        
 BEGIN             
 Insert into tblAssignUserDetail(UserID,OrderID,AssignDate,RectimeStamp)    
    SELECT CookID,B.ID,Getdate(),Getdate() FROM trnFoodOrderDetail A INNER JOIN #Assign B ON A.ID=B.ID 
   UPDATE trnFoodOrderDetail set PckID=B.UserId,AssignTo=CookID             
   FROM trnFoodOrderDetail A INNER JOIN #Assign B ON A.ID=B.ID 
   END        
    IF @Type='DB'        
 BEGIN             
   Insert into tblAssignUserDetail(UserID,OrderID,AssignDate,RectimeStamp)    
    SELECT PckID,B.ID,Getdate(),Getdate() FROM trnFoodOrderDetail A INNER JOIN #Assign B ON A.ID=B.ID 
    
   UPDATE trnFoodOrderDetail set DelvID=B.UserId,AssignTo=PckID                 
   FROM trnFoodOrderDetail A INNER JOIN #Assign B ON A.ID=B.ID 
   
  /* Insert into tblWorkStatus(UserId,OrderDetailId,OrderDate)         
	Select B.UserId,B.ID,A.OrderDate FROM trnFoodOrderDetail A   
	INNER JOIN #Assign B ON A.ID=B.ID */              
   END          
   
    IF @Type='SC'        
 BEGIN
 
 Insert into tblAssignUserDetail(UserID,OrderID,AssignDate,RectimeStamp)    
    SELECT DelvID,B.ID,Getdate(),Getdate() FROM trnFoodOrderDetail A INNER JOIN #Assign B ON A.ID=B.ID              
   UPDATE trnFoodOrderDetail set SCID=B.UserId,AssignTo=DelvID         
   FROM trnFoodOrderDetail A INNER JOIN #Assign B ON A.ID=B.ID 
    
   /*Insert into tblWorkStatus(UserId,OrderDetailId,OrderDate)         
	Select B.UserId,B.ID,A.OrderDate FROM trnFoodOrderDetail A   
	INNER JOIN #Assign B ON A.ID=B.ID*/               
   END          
           
   
             
     SELECT TOP 1 * from trnFoodOrderDetail          
              
  COMMIT TRANSACTION TestTransaction                                                              
                                    
                                                               
  --RETURN                                                              
                         
 END TRY                                                              
                                                             
BEGIN CATCH                                 
  SELECT @LocalError = ERROR_NUMBER()                                                              
   ,@ErrorMessage = ERROR_MESSAGE()                                                              
                                                              
  IF (XACT_STATE()) <> 0                                                              
  BEGIN                                                              
   ROLLBACK TRANSACTION TestTransaction                                                      
  END                                                              
                                                              
  RAISERROR (                                                              
    'TestSP: %d: %s'                                                              
    ,16                                                              
    ,1                                              
    ,@LocalError                                                              
    ,@ErrorMessage                                                              
    );                                                              
                                                              
  --RETURN (0)       
 END CATCH                                                           
  
            
END 



