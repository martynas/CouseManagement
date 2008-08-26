set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Adomas Paltanavicius
-- Create date: 11/09/08
-- Description:	Fetch new users for creation
-- =============================================
CREATE PROCEDURE [dbo].[cm_GetNewUsers] 
	-- Add the parameters for the stored procedure here
	@ApplicationName NVARCHAR(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Find application
	DECLARE @aspnet_ApplicationId UNIQUEIDENTIFIER;
	SELECT @aspnet_ApplicationId = 
		(
			SELECT ApplicationId
			FROM Atrendia_WEB.dbo.aspnet_Applications
			WHERE LoweredApplicationName = LOWER(@ApplicationName)
		);

	-- Fetch users from CDM, check against ASP.NET membership
	SELECT 
			cdmcont.id,
			cdmcont.email,
			cdmcont.title,
			cdmcont.firstN AS firstName,
			cdmcont.lastN AS lastName
		FROM 
			Atrendia_TEST.dbo.cdmcont
		WHERE
			cdmcont.Id IN -- Contact is assigned as primary contact for delivery package
			(
				SELECT DISTINCT(cdmcont.Id)
					FROM 
						Atrendia_TEST.dbo.DeliveryPackage
						INNER JOIN Atrendia_TEST.dbo.cdmcont 
							ON cdmcont.Id = DeliveryPackage.primaryContact
					WHERE
						DeliveryPackage.delT IS NULL -- Not deleted
						AND cdmcont.delT IS NULL
			)
			AND cdmcont.email NOT IN -- Contact is not registered as a user in web system
			(
				SELECT UserName
					FROM Atrendia_WEB.dbo.aspnet_Users
			);	
END

