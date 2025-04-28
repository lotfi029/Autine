using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using static Autine.Infrastructure.Identity.Consts.DefaultRoles;

namespace Autine.Infrastructure.Persistence.DBCommands;
public class StoredProcedures
{
    public static class BotSPs
    {
        public const string DeleteBotWithRelations = $"dbo.{nameof(DeleteBotWithRelations)}";
    }
    public static class BotPatientSPs
    {
        public const string DeleteBotPatientWithRelations = $"dbo.{nameof(DeleteBotPatientWithRelations)} @BotPatientId";

    }
    public static class BotMessageSPs
    {
        public const string DeleteBotMessagesWithRelations = $"dbo.{nameof(DeleteBotMessagesWithRelations)} @BotPatientId";
        public const string DeleteBotMessageParameter = "@BotPatientId";
        public static string DeleteBotMessagesWithRelationsProcedure => @"
            CREATE OR ALTER PROCEDURE dbo.DeleteBotMessagesWithRelations
                @BotPatientId UNIQUEIDENTIFIER
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @MessageIds TABLE (MessageId UNIQUEIDENTIFIER);

                -- Get all Message IDs linked to the BotPatient
                INSERT INTO @MessageIds (MessageId)
                SELECT BM.MessageId
                FROM BotMessages BM
                WHERE BM.BotPatientId = @BotPatientId;

                -- Delete BotMessages FIRST (this is crucial)
                -- BotMessages has the foreign key to Messages, so it must be deleted first
                DELETE FROM BotMessages
                WHERE BotPatientId = @BotPatientId;

                -- Now it's safe to delete Messages since no BotMessages references them anymore
                DELETE FROM Messages
                WHERE Id IN (SELECT MessageId FROM @MessageIds);

                RETURN 0;
            END;";

        public static string BotMessageDeleteTrigger => @"
            CREATE OR ALTER TRIGGER TR_BotMessage_Delete
            ON BotMessages
            INSTEAD OF DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @MessageIds TABLE (MessageId UNIQUEIDENTIFIER);

                -- Capture Message IDs from BotMessages being deleted
                INSERT INTO @MessageIds (MessageId)
                SELECT MessageId FROM deleted;

                -- First, delete the BotMessages
                DELETE FROM BotMessages
                WHERE MessageId IN (SELECT MessageId FROM @MessageIds);

                -- Then, delete the Messages (after the foreign key reference is gone)
                DELETE FROM Messages
                WHERE Id IN (SELECT MessageId FROM @MessageIds);
            END;";

    }

    public static class SupervisorSPs
    {
        public const string DeleteSupervisorRelations = $"dbo.{nameof(DeleteSupervisorRelations)}";
        public const string DeleteSupervisorRelationsCall = $"EXEC dbo.{nameof(DeleteSupervisorRelations)} @SupervisorId";
        public static SqlParameter DeleteSupervisorRelationsParamter(string supervisorId)
            => new("@SupervisorId", supervisorId);

        public static string DeleteSuperisorRelationsProcedure => @"
            IF OBJECT_ID('dbo.DeleteSupervisorRelations', 'P') IS NOT NULL
                DROP PROCEDURE dbo.DeleteSupervisorRelations;
            GO
            CREATE PROCEDURE dbo.DeleteSupervisorRelations
                @SupervisorId UNIQUEIDENTIFIER
            AS   
            BEGIN
                SET NOCOUNT, XACT_ABORT ON;
                BEGIN TRANSACTION;
                                    
                BEGIN TRY
        
                    DELETE tm
                    FROM dbo.ThreadMessages AS tm
                    INNER JOIN dbo.ThreadMembers AS m 
                        ON tm.ThreadMemberId = m.Id
                    INNER JOIN dbo.Patients AS p 
                        ON m.ThreadId = p.Id
                    WHERE p.CreatedBy = @SupervisorId;

        
                    DELETE bm
                    FROM dbo.BotMessages AS bm
                    INNER JOIN dbo.BotPatients AS bp 
                        ON bm.BotPatientId = bp.Id
                    INNER JOIN dbo.Patients AS p2 
                        ON bp.UserId = p2.PatientId
                    WHERE p2.CreatedBy = @SupervisorId;

        
                    ;WITH MsgsToDelete AS (
                        SELECT tm.MessageId
                        FROM dbo.ThreadMessages tm
                        JOIN dbo.ThreadMembers m ON tm.ThreadMemberId = m.Id
                        JOIN dbo.Patients p      ON m.ThreadId       = p.Id
                        WHERE p.CreatedBy = @SupervisorId

                        UNION ALL

                        SELECT bm.MessageId
                        FROM dbo.BotMessages bm
                        JOIN dbo.BotPatients bp ON bm.BotPatientId = bp.Id
                        JOIN dbo.Patients p2    ON bp.UserId        = p2.PatientId
                        WHERE p2.CreatedBy = @SupervisorId
                    )
                    DELETE msg
                    FROM dbo.Messages AS msg
                    JOIN MsgsToDelete d ON msg.Id = d.MessageId;

        
                    DELETE mbr
                    FROM dbo.ThreadMembers AS mbr
                    WHERE mbr.CreatedBy = @SupervisorId;

        
                    DELETE bp
                    FROM dbo.BotPatients AS bp
                    INNER JOIN dbo.Patients AS p3 
                        ON bp.UserId = p3.PatientId
                    WHERE p3.CreatedBy = @SupervisorId;

                    DELETE b
                    FROM dbo.Bots AS b
                    WHERE b.CreatedBy = @SupervisorId;

                    DELETE ur
		            FROM dbo.AspNetUserRoles    AS ur
		            INNER JOIN dbo.AspNetRoles   AS r  ON ur.RoleId = r.Id
		            INNER JOIN dbo.Patients      AS p  ON ur.UserId = p.PatientId
		            WHERE r.Name     = 'patient'
		              AND p.CreatedBy = @SupervisorId;
                    
                    DELETE pat
                    FROM dbo.Patients AS pat
                    WHERE pat.CreatedBy = @SupervisorId;

                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    IF XACT_STATE() <> 0
                        ROLLBACK TRANSACTION;

                    THROW;  
                END CATCH
            END
            GO";
    }

    public static class UserSPs
    {
        public const string DeleteUserWithRelation = $"dbo.{nameof(DeleteUserWithRelation)}";
        public const string DeleteUserWithRelationCall = $"EXEC dbo.{nameof(DeleteUserWithRelation)} @UserId";
        public static SqlParameter DeleteUserWithRelationParamter(string userId)
            => new("@UserId", userId);
        public static string DeleteUserWithRelationProdcedure => @"
            IF OBJECT_ID('dbo.DeleteUserWithRelation', 'P') IS NOT NULL
                DROP PROCEDURE dbo.DeleteUserWithRelation;
            GO
            CREATE PROCEDURE dbo.DeleteUserWithRelation
                @UserId NVARCHAR(450)
            AS
            BEGIN
                SET NOCOUNT, XACT_ABORT ON;
                BEGIN TRANSACTION;
                BEGIN TRY
		            
                    DELETE bm
		            from dbo.BotMessages as bm
		            inner join dbo.BotPatients as bp
			            on bp.Id = bm.BotPatientId
		            where bp.UserId = @UserId
		
		            DELETE msg
		            from dbo.Messages as msg
		            inner join BotMessages as bm
			            on msg.Id = bm.MessageId
		            inner join dbo.BotPatients as bp
			            on bp.Id = bm.BotPatientId
		            where bp.UserId = @UserId


		            DELETE bp
		            from dbo.BotPatients as bp
		            where bp.UserId = @UserId

                    DELETE FROM dbo.AspNetUserTokens 
                    WHERE UserId = @UserId;

                    DELETE FROM dbo.AspNetUserLogins 
                    WHERE UserId = @UserId;

                    DELETE FROM dbo.AspNetUserRoles  
                    WHERE UserId = @UserId;

                    DELETE FROM dbo.AspNetUserClaims 
                    WHERE UserId = @UserId;

                    DELETE FROM dbo.AspNetUsers      
                    WHERE Id = @UserId;

                    COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    IF XACT_STATE() <> 0
                        ROLLBACK TRANSACTION;
                    THROW;  -- re‐throw the original error
                END CATCH
            END
            GO";
    }
    public static class PatientSPs
    {
        public const string DeletePatientWithRelation = $"dbo.{nameof(DeletePatientWithRelation)}";
        public const string DeletePatientWithRelationCall = $"EXEC dbo.{nameof(DeletePatientWithRelation)} @PatientId";
        public static SqlParameter DeletePatientWithRelationParamter(string patientId)
            => new("@PatientId", patientId);
        public static string DeletePatientWithRelationProdcedure => @"
            IF OBJECT_ID('dbo.DeletePatientWithRelation', 'P') IS NOT NULL
                DROP PROCEDURE dbo.DeletePatientWithRelation;
            GO
            create procedure dbo.DeletePatientWithRelation
	            @PatientId NVARCHAR(450)
            AS
            BEGIN
                SET NOCOUNT, XACT_ABORT ON;
                BEGIN TRANSACTION;
                BEGIN TRY

		            DELETE bm
		            from dbo.BotMessages as bm
		            inner join dbo.BotPatients as bp
			            on bp.Id = bm.BotPatientId
		            where bp.UserId = @PatientId;
	
		            DELETE msg
		            from dbo.Messages as msg
		            inner join BotMessages as bm
			            on msg.Id = bm.MessageId
		            inner join dbo.BotPatients as bp
			            on bp.Id = bm.BotPatientId
		            where bp.UserId = @PatientId;

		            DELETE bp
		            from dbo.BotPatients as bp
		            where bp.UserId = @PatientId;

		            DELETE p
		            from dbo.Patients as p
		            where p.PatientId = @PatientId;

                    DELETE FROM dbo.AspNetUserTokens 
                    WHERE UserId = @PatientId;

                    DELETE FROM dbo.AspNetUserLogins 
                    WHERE UserId = @PatientId;

                    DELETE FROM dbo.AspNetUserRoles  
                    WHERE UserId = @PatientId;

                    DELETE FROM dbo.AspNetUserClaims 
                    WHERE UserId = @PatientId;

                    DELETE FROM dbo.AspNetUsers      
                    WHERE Id = @PatientId;
                  COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    IF XACT_STATE() <> 0
                        ROLLBACK TRANSACTION;
                    THROW;  -- re‐throw the original error
                END CATCH
            END
            GO";
    }
    public static class AdminSPs
    {
        public const string DeleteAdminWithRelation = $"dbo.{nameof(DeleteAdminWithRelation)}";
        public const string DeleteAdminWithRelationCall = $"EXEC dbo.{nameof(DeleteAdminWithRelation)} @AdminId";
        public static SqlParameter DeleteAdminWithRelationParamter(string adminId)
            => new("@AdminId", adminId);
        public static string DeleteAdminWithRelationProdcedure => @"
            IF OBJECT_ID('dbo.DeleteAdminWithRelation', 'P') IS NOT NULL
                DROP PROCEDURE dbo.DeleteAdminWithRelation;
            GO
            create procedure dbo.DeleteAdminWithRelation
	            @AdminId NVARCHAR(450)
            AS
            BEGIN
                SET NOCOUNT, XACT_ABORT ON;
                BEGIN TRANSACTION;
                BEGIN TRY
		
		            DELETE bm
		            from dbo.BotMessages as bm
		            inner join dbo.BotPatients as bp on bm.BotPatientId = bp.Id
		            inner join dbo.Bots as b on b.Id = bp.BotId
		            where b.CreatedBy = @AdminId

		            ;WITH MsgsToDelete AS (
                        SELECT bm.MessageId
                        FROM dbo.BotMessages bm
                        JOIN dbo.BotPatients bp ON bm.BotPatientId = bp.Id
                        JOIN dbo.Patients p2    ON bp.UserId        = p2.PatientId
                        WHERE p2.CreatedBy = @AdminId
                    )
                    DELETE msg
                    FROM dbo.Messages AS msg
                    JOIN MsgsToDelete d ON msg.Id = d.MessageId;

		            DELETE b
		            from dbo.Bots as b
		            where b.CreatedBy = @AdminId

                    DELETE FROM dbo.AspNetUserTokens 
                    WHERE UserId = @AdminId;

                    DELETE FROM dbo.AspNetUserLogins 
                    WHERE UserId = @AdminId;

                    DELETE FROM dbo.AspNetUserRoles  
                    WHERE UserId = @AdminId;

                    DELETE FROM dbo.AspNetUserClaims 
                    WHERE UserId = @AdminId;

                    DELETE FROM dbo.AspNetUsers      
                    WHERE Id = @AdminId;
                  COMMIT TRANSACTION;
                END TRY
                BEGIN CATCH
                    IF XACT_STATE() <> 0
                        ROLLBACK TRANSACTION;
                    THROW;  -- re‐throw the original error
                END CATCH
            END
            GO";
    }
}