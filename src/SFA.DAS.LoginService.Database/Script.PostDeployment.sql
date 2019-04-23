/*
	This master script is run on each database deployment; each child post deployment script will be run; and each must have been written
	such that it can run repeatably without affecting the integrity of an existing database.
*/
:r .\PostDeploymentScripts\InitialSetupData.sql

:r .\PostDeploymentScripts\UpdateEmailConfirmed.sql

:r .\PostDeploymentScripts\UpdateProfileClaims.sql

:r .\PostDeploymentScripts\UpdateEmailTemplates.sql
