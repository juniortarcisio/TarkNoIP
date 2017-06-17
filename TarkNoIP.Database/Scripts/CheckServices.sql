
SELECT *
FROM dbo.Service SC
JOIN dbo.Server SV ON SC.Id = SV.ServiceId 
JOIN dbo.Address AD ON AD.ServerId = SV.Id


--INSERT INTO Service VALUES (2, 'Where in the World')

--INSERT INTO Server VALUES ('Tarcisio WWAG Server', 'Welcome to my Where in the World is Ayam Goreng, I hope you enjoy it', 2, GETDATE())

--INSERT INTO Address VALUES ('189.110.213.34', 8, GETDATE())


SELECT *
FROM dbo.Service SC

SELECT *
FROM dbo.Server SV

SELECT *
FROM dbo.Address SV