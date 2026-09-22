-- Import generated from Book1.xlsx and matched with the supplied project export.
-- Project No. is preserved from column B; the H### prefix is removed from Company Name.
-- Existing matching records receive an available Go-Live date; missing dates remain unchanged.
SET XACT_ABORT ON;
BEGIN TRANSACTION;

DECLARE @ImportedAt datetime2 = SYSUTCDATETIME();

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H016' AND [CustomerName] = N'Huhtamaki (UK) Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2008-04-16' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H016' AND [CustomerName] = N'Huhtamaki (UK) Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Huhtamaki (UK) Ltd', N'', N'', N'H016', N'', N'', N'', N'', CAST('2008-04-16' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H017' AND [CustomerName] = N'TATA Consumer Products')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2009-03-09' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H017' AND [CustomerName] = N'TATA Consumer Products';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'TATA Consumer Products', N'', N'', N'H017', N'', N'', N'', N'', CAST('2009-03-09' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H019' AND [CustomerName] = N'Britvic EMEA Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2008-12-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H019' AND [CustomerName] = N'Britvic EMEA Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Britvic EMEA Limited', N'', N'', N'H019', N'', N'', N'', N'', CAST('2008-12-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H020' AND [CustomerName] = N'Vicon Industries Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2008-09-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H020' AND [CustomerName] = N'Vicon Industries Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Vicon Industries Ltd', N'', N'', N'H020', N'', N'', N'', N'', CAST('2008-09-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H021' AND [CustomerName] = N'Bruichladdich Distillers Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2009-05-13' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H021' AND [CustomerName] = N'Bruichladdich Distillers Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Bruichladdich Distillers Ltd', N'', N'', N'H021', N'', N'', N'', N'', CAST('2009-05-13' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H023' AND [CustomerName] = N'Eastland Compounding')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2009-01-02' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H023' AND [CustomerName] = N'Eastland Compounding';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Eastland Compounding', N'', N'', N'H023', N'', N'', N'', N'', CAST('2009-01-02' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H024' AND [CustomerName] = N'James Cropper Plc')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2009-10-28' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H024' AND [CustomerName] = N'James Cropper Plc';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'James Cropper Plc', N'', N'', N'H024', N'', N'', N'', N'', CAST('2009-10-28' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H025' AND [CustomerName] = N'Villeroy & Boch (Formerly Ideal Standard )')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2010-05-10' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H025' AND [CustomerName] = N'Villeroy & Boch (Formerly Ideal Standard )';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Villeroy & Boch (Formerly Ideal Standard )', N'', N'', N'H025', N'', N'', N'', N'', CAST('2010-05-10' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H028' AND [CustomerName] = N'BenRiach Distillery Co Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2011-06-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H028' AND [CustomerName] = N'BenRiach Distillery Co Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'BenRiach Distillery Co Ltd', N'', N'', N'H028', N'', N'', N'', N'', CAST('2011-06-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H030' AND [CustomerName] = N'Essentra Components')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2010-12-12' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H030' AND [CustomerName] = N'Essentra Components';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Essentra Components', N'', N'', N'H030', N'', N'', N'', N'', CAST('2010-12-12' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H036' AND [CustomerName] = N'CVH Spirits Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2013-03-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H036' AND [CustomerName] = N'CVH Spirits Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'CVH Spirits Limited', N'', N'', N'H036', N'', N'', N'', N'', CAST('2013-03-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H038' AND [CustomerName] = N'VWR International')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2011-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H038' AND [CustomerName] = N'VWR International';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'VWR International', N'', N'', N'H038', N'', N'', N'', N'', CAST('2011-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H040' AND [CustomerName] = N'Kilchoman Distillery Co.Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2011-03-28' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H040' AND [CustomerName] = N'Kilchoman Distillery Co.Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Kilchoman Distillery Co.Ltd', N'', N'', N'H040', N'', N'', N'', N'', CAST('2011-03-28' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H041' AND [CustomerName] = N'Culpitt Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2011-10-03' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H041' AND [CustomerName] = N'Culpitt Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Culpitt Ltd', N'', N'', N'H041', N'', N'', N'', N'', CAST('2011-10-03' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H042' AND [CustomerName] = N'British Sugar Plc')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2011-10-25' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H042' AND [CustomerName] = N'British Sugar Plc';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'British Sugar Plc', N'', N'', N'H042', N'', N'', N'', N'', CAST('2011-10-25' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H043' AND [CustomerName] = N'Thatchers Cider Company Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2011-11-15' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H043' AND [CustomerName] = N'Thatchers Cider Company Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Thatchers Cider Company Ltd', N'', N'', N'H043', N'', N'', N'', N'', CAST('2011-11-15' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H044' AND [CustomerName] = N'Haltermann Carless UK Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2012-01-06' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H044' AND [CustomerName] = N'Haltermann Carless UK Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Haltermann Carless UK Limited', N'', N'', N'H044', N'', N'', N'', N'', CAST('2012-01-06' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H046' AND [CustomerName] = N'Calrec Audio Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2014-01-20' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H046' AND [CustomerName] = N'Calrec Audio Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Calrec Audio Ltd', N'', N'', N'H046', N'', N'', N'', N'', CAST('2014-01-20' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H047' AND [CustomerName] = N'Renold Gears')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2012-12-05' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H047' AND [CustomerName] = N'Renold Gears';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Renold Gears', N'', N'', N'H047', N'', N'', N'', N'', CAST('2012-12-05' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H048' AND [CustomerName] = N'Innospec Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2013-01-21' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H048' AND [CustomerName] = N'Innospec Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Innospec Ltd', N'', N'', N'H048', N'', N'', N'', N'', CAST('2013-01-21' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H049' AND [CustomerName] = N'Hepworth & Co')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2013-08-15' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H049' AND [CustomerName] = N'Hepworth & Co';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Hepworth & Co', N'', N'', N'H049', N'', N'', N'', N'', CAST('2013-08-15' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H050' AND [CustomerName] = N'Brunel Healthcare Manufacturing Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2014-08-22' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H050' AND [CustomerName] = N'Brunel Healthcare Manufacturing Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Brunel Healthcare Manufacturing Ltd', N'', N'', N'H050', N'', N'', N'', N'', CAST('2014-08-22' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H053' AND [CustomerName] = N'ACEO Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2014-05-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H053' AND [CustomerName] = N'ACEO Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'ACEO Limited', N'', N'', N'H053', N'', N'', N'', N'', CAST('2014-05-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H055' AND [CustomerName] = N'Johnson Tiles Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2015-10-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H055' AND [CustomerName] = N'Johnson Tiles Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Johnson Tiles Ltd', N'', N'', N'H055', N'', N'', N'', N'', CAST('2015-10-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H057' AND [CustomerName] = N'Tata Steel UK Ltd (CATNIC)')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2014-12-19' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H057' AND [CustomerName] = N'Tata Steel UK Ltd (CATNIC)';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Tata Steel UK Ltd (CATNIC)', N'', N'', N'H057', N'', N'', N'', N'', CAST('2014-12-19' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H058' AND [CustomerName] = N'Haygrove Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2015-07-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H058' AND [CustomerName] = N'Haygrove Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Haygrove Ltd', N'', N'', N'H058', N'', N'', N'', N'', CAST('2015-07-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H060' AND [CustomerName] = N'Butcombe Brewery')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2015-04-21' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H060' AND [CustomerName] = N'Butcombe Brewery';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Butcombe Brewery', N'', N'', N'H060', N'', N'', N'', N'', CAST('2015-04-21' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H061' AND [CustomerName] = N'Interfloor Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2015-12-14' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H061' AND [CustomerName] = N'Interfloor Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Interfloor Ltd', N'', N'', N'H061', N'', N'', N'', N'', CAST('2015-12-14' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H062' AND [CustomerName] = N'Mettis Aerospace')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2015-12-07' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H062' AND [CustomerName] = N'Mettis Aerospace';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Mettis Aerospace', N'', N'', N'H062', N'', N'', N'', N'', CAST('2015-12-07' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H065' AND [CustomerName] = N'Heroux Devtek')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2016-10-03' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H065' AND [CustomerName] = N'Heroux Devtek';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Heroux Devtek', N'', N'', N'H065', N'', N'', N'', N'', CAST('2016-10-03' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H067' AND [CustomerName] = N'Belzona Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2017-09-18' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H067' AND [CustomerName] = N'Belzona Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Belzona Limited', N'', N'', N'H067', N'', N'', N'', N'', CAST('2017-09-18' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H069' AND [CustomerName] = N'Coveris Flexibles UK Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2018-03-21' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H069' AND [CustomerName] = N'Coveris Flexibles UK Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Coveris Flexibles UK Limited', N'', N'', N'H069', N'', N'', N'', N'', CAST('2018-03-21' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H070' AND [CustomerName] = N'Apollo Fire Detectors')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2018-01-15' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H070' AND [CustomerName] = N'Apollo Fire Detectors';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Apollo Fire Detectors', N'', N'', N'H070', N'', N'', N'', N'', CAST('2018-01-15' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H072' AND [CustomerName] = N'Speciality Steel UK Limited– In Liquidation')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H072' AND [CustomerName] = N'Speciality Steel UK Limited– In Liquidation';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Speciality Steel UK Limited– In Liquidation', N'', N'', N'H072', N'', N'', N'', N'', CAST('2026-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H074' AND [CustomerName] = N'Amann Group')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-05-28' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H074' AND [CustomerName] = N'Amann Group';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Amann Group', N'', N'', N'H074', N'', N'', N'', N'', CAST('2021-05-28' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H075' AND [CustomerName] = N'Vickers Oil')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2019-02-27' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H075' AND [CustomerName] = N'Vickers Oil';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Vickers Oil', N'', N'', N'H075', N'', N'', N'', N'', CAST('2019-02-27' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H076' AND [CustomerName] = N'InchDairnie Distillery Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2019-04-04' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H076' AND [CustomerName] = N'InchDairnie Distillery Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'InchDairnie Distillery Ltd', N'', N'', N'H076', N'', N'', N'', N'', CAST('2019-04-04' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H078' AND [CustomerName] = N'Weir Morocco (N Africa)')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2019-06-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H078' AND [CustomerName] = N'Weir Morocco (N Africa)';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Weir Morocco (N Africa)', N'', N'', N'H078', N'', N'', N'', N'', CAST('2019-06-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H081' AND [CustomerName] = N'Integrity Print')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H081' AND [CustomerName] = N'Integrity Print';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Integrity Print', N'', N'', N'H081', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H082' AND [CustomerName] = N'Middle East Chemicals')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2019-08-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H082' AND [CustomerName] = N'Middle East Chemicals';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Middle East Chemicals', N'', N'', N'H082', N'', N'', N'', N'', CAST('2019-08-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H084' AND [CustomerName] = N'LAVAZZA Professional')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2019-10-15' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H084' AND [CustomerName] = N'LAVAZZA Professional';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'LAVAZZA Professional', N'', N'', N'H084', N'', N'', N'', N'', CAST('2019-10-15' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H085' AND [CustomerName] = N'Spear UK')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H085' AND [CustomerName] = N'Spear UK';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Spear UK', N'', N'', N'H085', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H086' AND [CustomerName] = N'Weir Minerals Europe Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-10-06' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H086' AND [CustomerName] = N'Weir Minerals Europe Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Weir Minerals Europe Limited', N'', N'', N'H086', N'', N'', N'', N'', CAST('2021-10-06' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H087' AND [CustomerName] = N'Peter Green Chilled')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2020-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H087' AND [CustomerName] = N'Peter Green Chilled';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Peter Green Chilled', N'', N'', N'H087', N'', N'', N'', N'', CAST('2020-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H091' AND [CustomerName] = N'Hunter Laing & Company Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2020-01-07' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H091' AND [CustomerName] = N'Hunter Laing & Company Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Hunter Laing & Company Limited', N'', N'', N'H091', N'', N'', N'', N'', CAST('2020-01-07' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H092' AND [CustomerName] = N'Dispak Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H092' AND [CustomerName] = N'Dispak Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Dispak Ltd', N'', N'', N'H092', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H093' AND [CustomerName] = N'Donaldson Filter Components')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2020-04-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H093' AND [CustomerName] = N'Donaldson Filter Components';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Donaldson Filter Components', N'', N'', N'H093', N'', N'', N'', N'', CAST('2020-04-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H096' AND [CustomerName] = N'Interior Products Group')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2020-03-09' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H096' AND [CustomerName] = N'Interior Products Group';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Interior Products Group', N'', N'', N'H096', N'', N'', N'', N'', CAST('2020-03-09' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H098' AND [CustomerName] = N'Benchmark Packaging Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2020-03-30' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H098' AND [CustomerName] = N'Benchmark Packaging Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Benchmark Packaging Ltd', N'', N'', N'H098', N'', N'', N'', N'', CAST('2020-03-30' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H099' AND [CustomerName] = N'Performance Plastics')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-07-22' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H099' AND [CustomerName] = N'Performance Plastics';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Performance Plastics', N'', N'', N'H099', N'', N'', N'', N'', CAST('2021-07-22' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H105' AND [CustomerName] = N'ChamberCustoms Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2020-06-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H105' AND [CustomerName] = N'ChamberCustoms Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'ChamberCustoms Limited', N'', N'', N'H105', N'', N'', N'', N'', CAST('2020-06-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H107' AND [CustomerName] = N'Fidelity Supply Chain Solutions Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H107' AND [CustomerName] = N'Fidelity Supply Chain Solutions Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Fidelity Supply Chain Solutions Ltd', N'', N'', N'H107', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H111' AND [CustomerName] = N'Autosmart International')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H111' AND [CustomerName] = N'Autosmart International';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Autosmart International', N'', N'', N'H111', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H112' AND [CustomerName] = N'Independent Freight')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H112' AND [CustomerName] = N'Independent Freight';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Independent Freight', N'', N'', N'H112', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H113' AND [CustomerName] = N'Fireboy Xintex UK Operations Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H113' AND [CustomerName] = N'Fireboy Xintex UK Operations Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Fireboy Xintex UK Operations Ltd', N'', N'', N'H113', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H116' AND [CustomerName] = N'Kawasaki Precision Machinery (UK)')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H116' AND [CustomerName] = N'Kawasaki Precision Machinery (UK)';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Kawasaki Precision Machinery (UK)', N'', N'', N'H116', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H117' AND [CustomerName] = N'John Hogg Technical Solutions')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H117' AND [CustomerName] = N'John Hogg Technical Solutions';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'John Hogg Technical Solutions', N'', N'', N'H117', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H118' AND [CustomerName] = N'RPC Containers (Corby)')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H118' AND [CustomerName] = N'RPC Containers (Corby)';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'RPC Containers (Corby)', N'', N'', N'H118', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H120' AND [CustomerName] = N'Carling Technologies Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H120' AND [CustomerName] = N'Carling Technologies Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Carling Technologies Ltd', N'', N'', N'H120', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H122' AND [CustomerName] = N'Britvic Soft Drinks Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H122' AND [CustomerName] = N'Britvic Soft Drinks Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Britvic Soft Drinks Ltd', N'', N'', N'H122', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H125' AND [CustomerName] = N'Mollertech Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H125' AND [CustomerName] = N'Mollertech Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Mollertech Limited', N'', N'', N'H125', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H126' AND [CustomerName] = N'Seneca Environmental Solutions Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H126' AND [CustomerName] = N'Seneca Environmental Solutions Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Seneca Environmental Solutions Ltd', N'', N'', N'H126', N'', N'', N'', N'', CAST('2021-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H127' AND [CustomerName] = N'Globe Customs Services Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-05-13' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H127' AND [CustomerName] = N'Globe Customs Services Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Globe Customs Services Ltd', N'', N'', N'H127', N'', N'', N'', N'', CAST('2021-05-13' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H129' AND [CustomerName] = N'Castings PLC')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-06-15' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H129' AND [CustomerName] = N'Castings PLC';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Castings PLC', N'', N'', N'H129', N'', N'', N'', N'', CAST('2021-06-15' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H132' AND [CustomerName] = N'Sovereign Beverage Company')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-03-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H132' AND [CustomerName] = N'Sovereign Beverage Company';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Sovereign Beverage Company', N'', N'', N'H132', N'', N'', N'', N'', CAST('2021-03-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H135' AND [CustomerName] = N'MCT Reman Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-02-17' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H135' AND [CustomerName] = N'MCT Reman Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'MCT Reman Ltd', N'', N'', N'H135', N'', N'', N'', N'', CAST('2021-02-17' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H136' AND [CustomerName] = N'Cave Direct Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-02-18' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H136' AND [CustomerName] = N'Cave Direct Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Cave Direct Ltd', N'', N'', N'H136', N'', N'', N'', N'', CAST('2021-02-18' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H139' AND [CustomerName] = N'James Dawson & Son Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-03-09' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H139' AND [CustomerName] = N'James Dawson & Son Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'James Dawson & Son Ltd', N'', N'', N'H139', N'', N'', N'', N'', CAST('2021-03-09' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H142' AND [CustomerName] = N'Andusia Recovered Fuels')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-04-09' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H142' AND [CustomerName] = N'Andusia Recovered Fuels';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Andusia Recovered Fuels', N'', N'', N'H142', N'', N'', N'', N'', CAST('2021-04-09' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H144' AND [CustomerName] = N'Precision Hydraulic Cylinders')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-04-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H144' AND [CustomerName] = N'Precision Hydraulic Cylinders';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Precision Hydraulic Cylinders', N'', N'', N'H144', N'', N'', N'', N'', CAST('2021-04-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H145' AND [CustomerName] = N'Dunster House Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-04-13' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H145' AND [CustomerName] = N'Dunster House Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Dunster House Ltd', N'', N'', N'H145', N'', N'', N'', N'', CAST('2021-04-13' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H149' AND [CustomerName] = N'Associated Waste Management')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-05-11' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H149' AND [CustomerName] = N'Associated Waste Management';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Associated Waste Management', N'', N'', N'H149', N'', N'', N'', N'', CAST('2021-05-11' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H151' AND [CustomerName] = N'FC Brown (Steel Equip) Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-05-20' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H151' AND [CustomerName] = N'FC Brown (Steel Equip) Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'FC Brown (Steel Equip) Ltd', N'', N'', N'H151', N'', N'', N'', N'', CAST('2021-05-20' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H152' AND [CustomerName] = N'Eurosprint Freight Services')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-04-08' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H152' AND [CustomerName] = N'Eurosprint Freight Services';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Eurosprint Freight Services', N'', N'', N'H152', N'', N'', N'', N'', CAST('2021-04-08' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H156' AND [CustomerName] = N'Best Foods Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-06-10' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H156' AND [CustomerName] = N'Best Foods Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Best Foods Limited', N'', N'', N'H156', N'', N'', N'', N'', CAST('2021-06-10' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H159' AND [CustomerName] = N'AGA Rangemaster Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-07-13' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H159' AND [CustomerName] = N'AGA Rangemaster Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'AGA Rangemaster Limited', N'', N'', N'H159', N'', N'', N'', N'', CAST('2021-07-13' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H164' AND [CustomerName] = N'Katmex')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-06-17' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H164' AND [CustomerName] = N'Katmex';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Katmex', N'', N'', N'H164', N'', N'', N'', N'', CAST('2021-06-17' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H166' AND [CustomerName] = N'Morrison Scotch Whisky Distillers Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-07-04' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H166' AND [CustomerName] = N'Morrison Scotch Whisky Distillers Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Morrison Scotch Whisky Distillers Limited', N'', N'', N'H166', N'', N'', N'', N'', CAST('2022-07-04' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H167' AND [CustomerName] = N'Doccombe Global Logistics')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-08-03' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H167' AND [CustomerName] = N'Doccombe Global Logistics';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Doccombe Global Logistics', N'', N'', N'H167', N'', N'', N'', N'', CAST('2021-08-03' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H169' AND [CustomerName] = N'Remsons Automotive Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-09-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H169' AND [CustomerName] = N'Remsons Automotive Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Remsons Automotive Ltd', N'', N'', N'H169', N'', N'', N'', N'', CAST('2021-09-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H170' AND [CustomerName] = N'Americold Whitchurch Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-08-12' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H170' AND [CustomerName] = N'Americold Whitchurch Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Americold Whitchurch Limited', N'', N'', N'H170', N'', N'', N'', N'', CAST('2021-08-12' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H171' AND [CustomerName] = N'Ewals Cargo Care B.V')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-09-17' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H171' AND [CustomerName] = N'Ewals Cargo Care B.V';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Ewals Cargo Care B.V', N'', N'', N'H171', N'', N'', N'', N'', CAST('2021-09-17' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H172' AND [CustomerName] = N'JTI / Gallaher')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-02-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H172' AND [CustomerName] = N'JTI / Gallaher';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'JTI / Gallaher', N'', N'', N'H172', N'', N'', N'', N'', CAST('2022-02-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H173' AND [CustomerName] = N'Si Logistics Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-11-09' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H173' AND [CustomerName] = N'Si Logistics Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Si Logistics Limited', N'', N'', N'H173', N'', N'', N'', N'', CAST('2021-11-09' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H174' AND [CustomerName] = N'Strong and Herd LLP')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-11-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H174' AND [CustomerName] = N'Strong and Herd LLP';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Strong and Herd LLP', N'', N'', N'H174', N'', N'', N'', N'', CAST('2021-11-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H175' AND [CustomerName] = N'JG Pears Newark LTD')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2021-12-09' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H175' AND [CustomerName] = N'JG Pears Newark LTD';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'JG Pears Newark LTD', N'', N'', N'H175', N'', N'', N'', N'', CAST('2021-12-09' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H176' AND [CustomerName] = N'The GlenAllachie Distillers Co Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-06-23' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H176' AND [CustomerName] = N'The GlenAllachie Distillers Co Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'The GlenAllachie Distillers Co Ltd', N'', N'', N'H176', N'', N'', N'', N'', CAST('2022-06-23' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H180' AND [CustomerName] = N'Glanbia Nutrition UK Ltd (Middlesborough)')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-02-15' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H180' AND [CustomerName] = N'Glanbia Nutrition UK Ltd (Middlesborough)';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Glanbia Nutrition UK Ltd (Middlesborough)', N'', N'', N'H180', N'', N'', N'', N'', CAST('2022-02-15' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H181' AND [CustomerName] = N'Creme d''Or Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-03-04' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H181' AND [CustomerName] = N'Creme d''Or Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Creme d''Or Limited', N'', N'', N'H181', N'', N'', N'', N'', CAST('2022-03-04' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H182' AND [CustomerName] = N'Loadhog Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-03-10' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H182' AND [CustomerName] = N'Loadhog Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Loadhog Ltd', N'', N'', N'H182', N'', N'', N'', N'', CAST('2022-03-10' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H183' AND [CustomerName] = N'City Metals Recycling LTD')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-03-15' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H183' AND [CustomerName] = N'City Metals Recycling LTD';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'City Metals Recycling LTD', N'', N'', N'H183', N'', N'', N'', N'', CAST('2022-03-15' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H184' AND [CustomerName] = N'Cores Environment UK LTD')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-09-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H184' AND [CustomerName] = N'Cores Environment UK LTD';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Cores Environment UK LTD', N'', N'', N'H184', N'', N'', N'', N'', CAST('2022-09-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H188' AND [CustomerName] = N'Fujifilm Electronic Materials Uk Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-04-19' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H188' AND [CustomerName] = N'Fujifilm Electronic Materials Uk Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Fujifilm Electronic Materials Uk Limited', N'', N'', N'H188', N'', N'', N'', N'', CAST('2022-04-19' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H189' AND [CustomerName] = N'GTS Logistics UK Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-07-27' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H189' AND [CustomerName] = N'GTS Logistics UK Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'GTS Logistics UK Ltd', N'', N'', N'H189', N'', N'', N'', N'', CAST('2022-07-27' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H190' AND [CustomerName] = N'Boom Customs Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-06-27' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H190' AND [CustomerName] = N'Boom Customs Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Boom Customs Ltd', N'', N'', N'H190', N'', N'', N'', N'', CAST('2022-06-27' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H192' AND [CustomerName] = N'Exporter Services')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H192' AND [CustomerName] = N'Exporter Services';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Exporter Services', N'', N'', N'H192', N'', N'', N'', N'', CAST('2023-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H193' AND [CustomerName] = N'Scottish Leather Group')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-09-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H193' AND [CustomerName] = N'Scottish Leather Group';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Scottish Leather Group', N'', N'', N'H193', N'', N'', N'', N'', CAST('2023-09-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H194' AND [CustomerName] = N'Universal Customs Clearance')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-09-21' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H194' AND [CustomerName] = N'Universal Customs Clearance';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Universal Customs Clearance', N'', N'', N'H194', N'', N'', N'', N'', CAST('2022-09-21' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H195' AND [CustomerName] = N'ECU Worldwide UK Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-10-13' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H195' AND [CustomerName] = N'ECU Worldwide UK Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'ECU Worldwide UK Ltd', N'', N'', N'H195', N'', N'', N'', N'', CAST('2022-10-13' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H196' AND [CustomerName] = N'Maguire Europe Sales Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H196' AND [CustomerName] = N'Maguire Europe Sales Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Maguire Europe Sales Limited', N'', N'', N'H196', N'', N'', N'', N'', CAST('2023-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H198' AND [CustomerName] = N'Sedamyl UK Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-12-13' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H198' AND [CustomerName] = N'Sedamyl UK Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Sedamyl UK Ltd', N'', N'', N'H198', N'', N'', N'', N'', CAST('2022-12-13' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H199' AND [CustomerName] = N'WHS Plastics Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-01-12' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H199' AND [CustomerName] = N'WHS Plastics Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'WHS Plastics Limited', N'', N'', N'H199', N'', N'', N'', N'', CAST('2023-01-12' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H200' AND [CustomerName] = N'Agrial Fresh Produce Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2022-12-06' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H200' AND [CustomerName] = N'Agrial Fresh Produce Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Agrial Fresh Produce Limited', N'', N'', N'H200', N'', N'', N'', N'', CAST('2022-12-06' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H202' AND [CustomerName] = N'Samuel Banner & Co Ltd t/a Banner Chemicals')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-02-23' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H202' AND [CustomerName] = N'Samuel Banner & Co Ltd t/a Banner Chemicals';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Samuel Banner & Co Ltd t/a Banner Chemicals', N'', N'', N'H202', N'', N'', N'', N'', CAST('2023-02-23' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H203' AND [CustomerName] = N'Rotor Blades Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-03-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H203' AND [CustomerName] = N'Rotor Blades Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Rotor Blades Ltd', N'', N'', N'H203', N'', N'', N'', N'', CAST('2023-03-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H205' AND [CustomerName] = N'SIG Trading')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-05-05' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H205' AND [CustomerName] = N'SIG Trading';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'SIG Trading', N'', N'', N'H205', N'', N'', N'', N'', CAST('2023-05-05' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H206' AND [CustomerName] = N'Soprema UK Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-05-15' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H206' AND [CustomerName] = N'Soprema UK Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Soprema UK Limited', N'', N'', N'H206', N'', N'', N'', N'', CAST('2023-05-15' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H207' AND [CustomerName] = N'Pendennis Shipyard Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-06-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H207' AND [CustomerName] = N'Pendennis Shipyard Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Pendennis Shipyard Limited', N'', N'', N'H207', N'', N'', N'', N'', CAST('2023-06-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H208' AND [CustomerName] = N'H20 Innovation (Genesys International)')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-02-15' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H208' AND [CustomerName] = N'H20 Innovation (Genesys International)';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'H20 Innovation (Genesys International)', N'', N'', N'H208', N'', N'', N'', N'', CAST('2024-02-15' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H209' AND [CustomerName] = N'Iceland Foods Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-10-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H209' AND [CustomerName] = N'Iceland Foods Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Iceland Foods Ltd', N'', N'', N'H209', N'', N'', N'', N'', CAST('2023-10-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H210' AND [CustomerName] = N'Clearfast Services Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-08-18' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H210' AND [CustomerName] = N'Clearfast Services Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Clearfast Services Ltd', N'', N'', N'H210', N'', N'', N'', N'', CAST('2023-08-18' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H211' AND [CustomerName] = N'Somerset Cider Solutions Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-07-11' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H211' AND [CustomerName] = N'Somerset Cider Solutions Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Somerset Cider Solutions Ltd', N'', N'', N'H211', N'', N'', N'', N'', CAST('2023-07-11' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H212' AND [CustomerName] = N'Premiership Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-09-11' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H212' AND [CustomerName] = N'Premiership Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Premiership Limited', N'', N'', N'H212', N'', N'', N'', N'', CAST('2023-09-11' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H214' AND [CustomerName] = N'REHAU Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2023-12-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H214' AND [CustomerName] = N'REHAU Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'REHAU Ltd', N'', N'', N'H214', N'', N'', N'', N'', CAST('2023-12-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H215' AND [CustomerName] = N'AVL United Kingdom Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H215' AND [CustomerName] = N'AVL United Kingdom Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'AVL United Kingdom Limited', N'', N'', N'H215', N'', N'', N'', N'', CAST('2024-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H216' AND [CustomerName] = N'Elixir Distillers Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-01-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H216' AND [CustomerName] = N'Elixir Distillers Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Elixir Distillers Ltd', N'', N'', N'H216', N'', N'', N'', N'', CAST('2024-01-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H217' AND [CustomerName] = N'Ball & Young Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-04-08' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H217' AND [CustomerName] = N'Ball & Young Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Ball & Young Ltd', N'', N'', N'H217', N'', N'', N'', N'', CAST('2024-04-08' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H218' AND [CustomerName] = N'Euro - Solution Services Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-01-12' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H218' AND [CustomerName] = N'Euro - Solution Services Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Euro - Solution Services Limited', N'', N'', N'H218', N'', N'', N'', N'', CAST('2024-01-12' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H219' AND [CustomerName] = N'Northern Monk Brewing Co Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-01-12' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H219' AND [CustomerName] = N'Northern Monk Brewing Co Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Northern Monk Brewing Co Ltd', N'', N'', N'H219', N'', N'', N'', N'', CAST('2024-01-12' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H220' AND [CustomerName] = N'Daily Import Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-03-11' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H220' AND [CustomerName] = N'Daily Import Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Daily Import Limited', N'', N'', N'H220', N'', N'', N'', N'', CAST('2024-03-11' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H221' AND [CustomerName] = N'Advanced Proteins Unlimited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-04-12' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H221' AND [CustomerName] = N'Advanced Proteins Unlimited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Advanced Proteins Unlimited', N'', N'', N'H221', N'', N'', N'', N'', CAST('2024-04-12' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H222' AND [CustomerName] = N'Firebox Global')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-06-03' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H222' AND [CustomerName] = N'Firebox Global';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Firebox Global', N'', N'', N'H222', N'', N'', N'', N'', CAST('2024-06-03' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H223' AND [CustomerName] = N'TMD Friction UK Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-06-20' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H223' AND [CustomerName] = N'TMD Friction UK Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'TMD Friction UK Ltd', N'', N'', N'H223', N'', N'', N'', N'', CAST('2024-06-20' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H224' AND [CustomerName] = N'Ferryspeed (CI) Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-07-23' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H224' AND [CustomerName] = N'Ferryspeed (CI) Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Ferryspeed (CI) Limited', N'', N'', N'H224', N'', N'', N'', N'', CAST('2024-07-23' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H225' AND [CustomerName] = N'Nash Logistics Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-08-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H225' AND [CustomerName] = N'Nash Logistics Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Nash Logistics Ltd', N'', N'', N'H225', N'', N'', N'', N'', CAST('2024-08-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H226' AND [CustomerName] = N'Tetrosyl Group Ltd')
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Tetrosyl Group Ltd', N'', N'', N'H226', N'', N'', N'', N'', NULL, N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H229' AND [CustomerName] = N'Anchor Freight Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-09-05' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H229' AND [CustomerName] = N'Anchor Freight Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Anchor Freight Ltd', N'', N'', N'H229', N'', N'', N'', N'', CAST('2024-09-05' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H230' AND [CustomerName] = N'BAND-IT Company Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2025-03-06' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H230' AND [CustomerName] = N'BAND-IT Company Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'BAND-IT Company Limited', N'', N'', N'H230', N'', N'', N'', N'', CAST('2025-03-06' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H231' AND [CustomerName] = N'HI-LEX ACT WALES LTD (formally Mitsui Compon')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2024-11-14' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H231' AND [CustomerName] = N'HI-LEX ACT WALES LTD (formally Mitsui Compon';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'HI-LEX ACT WALES LTD (formally Mitsui Compon', N'', N'', N'H231', N'', N'', N'', N'', CAST('2024-11-14' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H232' AND [CustomerName] = N'Relay Port Agency Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2025-03-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H232' AND [CustomerName] = N'Relay Port Agency Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Relay Port Agency Limited', N'', N'', N'H232', N'', N'', N'', N'', CAST('2025-03-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H233' AND [CustomerName] = N'SE Bollore Logistics (Ceva Logistics)')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2025-02-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H233' AND [CustomerName] = N'SE Bollore Logistics (Ceva Logistics)';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'SE Bollore Logistics (Ceva Logistics)', N'', N'', N'H233', N'', N'', N'', N'', CAST('2025-02-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H234' AND [CustomerName] = N'DE HAAS Road Cargo')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2025-02-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H234' AND [CustomerName] = N'DE HAAS Road Cargo';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'DE HAAS Road Cargo', N'', N'', N'H234', N'', N'', N'', N'', CAST('2025-02-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H235' AND [CustomerName] = N'Transogueta S.L.')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2025-02-20' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H235' AND [CustomerName] = N'Transogueta S.L.';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Transogueta S.L.', N'', N'', N'H235', N'', N'', N'', N'', CAST('2025-02-20' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H236' AND [CustomerName] = N'Sandvik Osprey Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2025-04-07' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H236' AND [CustomerName] = N'Sandvik Osprey Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Sandvik Osprey Ltd', N'', N'', N'H236', N'', N'', N'', N'', CAST('2025-04-07' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H237' AND [CustomerName] = N'So Good Logistics Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2025-05-24' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H237' AND [CustomerName] = N'So Good Logistics Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'So Good Logistics Limited', N'', N'', N'H237', N'', N'', N'', N'', CAST('2025-05-24' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H238' AND [CustomerName] = N'Grant & Bowman Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-04-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H238' AND [CustomerName] = N'Grant & Bowman Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Grant & Bowman Ltd', N'', N'', N'H238', N'', N'', N'', N'', CAST('2026-04-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H239' AND [CustomerName] = N'Kingsland Drinks Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-01-17' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H239' AND [CustomerName] = N'Kingsland Drinks Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Kingsland Drinks Limited', N'', N'', N'H239', N'', N'', N'', N'', CAST('2026-01-17' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H240' AND [CustomerName] = N'Volpecastello Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2025-12-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H240' AND [CustomerName] = N'Volpecastello Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Volpecastello Limited', N'', N'', N'H240', N'', N'', N'', N'', CAST('2025-12-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H241' AND [CustomerName] = N'Artigos Ltd')
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Artigos Ltd', N'', N'', N'H241', N'', N'', N'', N'', NULL, N'Active', @ImportedAt, @ImportedAt);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H242' AND [CustomerName] = N'Eurilait Limited')
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Eurilait Limited', N'', N'', N'H242', N'', N'', N'', N'', NULL, N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H243' AND [CustomerName] = N'Clearport Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-02-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H243' AND [CustomerName] = N'Clearport Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Clearport Ltd', N'', N'', N'H243', N'', N'', N'', N'', CAST('2026-02-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H244' AND [CustomerName] = N'Boskovski & Kernebeck GmbH & Co. KG')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-03-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H244' AND [CustomerName] = N'Boskovski & Kernebeck GmbH & Co. KG';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Boskovski & Kernebeck GmbH & Co. KG', N'', N'', N'H244', N'', N'', N'', N'', CAST('2026-03-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H245' AND [CustomerName] = N'Allenek Industries Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-03-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H245' AND [CustomerName] = N'Allenek Industries Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Allenek Industries Ltd', N'', N'', N'H245', N'', N'', N'', N'', CAST('2026-03-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H246' AND [CustomerName] = N'Unit One Store Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-03-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H246' AND [CustomerName] = N'Unit One Store Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Unit One Store Limited', N'', N'', N'H246', N'', N'', N'', N'', CAST('2026-03-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H247' AND [CustomerName] = N'BB&R Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-04-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H247' AND [CustomerName] = N'BB&R Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'BB&R Limited', N'', N'', N'H247', N'', N'', N'', N'', CAST('2026-04-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H248' AND [CustomerName] = N'Red Kite Customs Clearance Ltd')
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Red Kite Customs Clearance Ltd', N'', N'', N'H248', N'', N'', N'', N'', NULL, N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H249' AND [CustomerName] = N'SSI Schaefer Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-04-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H249' AND [CustomerName] = N'SSI Schaefer Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'SSI Schaefer Limited', N'', N'', N'H249', N'', N'', N'', N'', CAST('2026-04-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H250' AND [CustomerName] = N'On Tap Drinks Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-07-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H250' AND [CustomerName] = N'On Tap Drinks Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'On Tap Drinks Ltd', N'', N'', N'H250', N'', N'', N'', N'', CAST('2026-07-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H251' AND [CustomerName] = N'R.Twining and Company Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-06-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H251' AND [CustomerName] = N'R.Twining and Company Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'R.Twining and Company Limited', N'', N'', N'H251', N'', N'', N'', N'', CAST('2026-06-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H252' AND [CustomerName] = N'Maxline Logistics Limited')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-06-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H252' AND [CustomerName] = N'Maxline Logistics Limited';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Maxline Logistics Limited', N'', N'', N'H252', N'', N'', N'', N'', CAST('2026-06-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H253' AND [CustomerName] = N'Ian Macleod Distillers Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-09-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H253' AND [CustomerName] = N'Ian Macleod Distillers Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Ian Macleod Distillers Ltd', N'', N'', N'H253', N'', N'', N'', N'', CAST('2026-09-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H254' AND [CustomerName] = N'Ship Express Logistics Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-08-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H254' AND [CustomerName] = N'Ship Express Logistics Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Ship Express Logistics Ltd', N'', N'', N'H254', N'', N'', N'', N'', CAST('2026-08-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'H255' AND [CustomerName] = N'Ignis Circular Ltd')
BEGIN
    UPDATE [dbo].[ExistingCustomers] SET [GoLiveDate] = CAST('2026-09-01' AS datetime2), [UpdatedAt] = @ImportedAt WHERE [ProjectNumber] = N'H255' AND [CustomerName] = N'Ignis Circular Ltd';
END
ELSE
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'Ignis Circular Ltd', N'', N'', N'H255', N'', N'', N'', N'', CAST('2026-09-01' AS datetime2), N'Active', @ImportedAt, @ImportedAt);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'NULL' AND [CustomerName] = N'I002 - Core')
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'I002 - Core', N'', N'', N'NULL', N'', N'', N'', N'', NULL, N'Active', @ImportedAt, @ImportedAt);
END

IF NOT EXISTS (SELECT 1 FROM [dbo].[ExistingCustomers] WHERE [ProjectNumber] = N'K001' AND [CustomerName] = N'K001 - ChamberCustoms')
BEGIN
    INSERT INTO [dbo].[ExistingCustomers] ([CustomerName], [ContactName], [ContactEmail], [ProjectNumber], [ProductsPurchased], [SoftwareDetails], [ImplementationOwner], [ClientNumber], [GoLiveDate], [Status], [CreatedAt], [UpdatedAt])
    VALUES (N'K001 - ChamberCustoms', N'', N'', N'K001', N'', N'', N'', N'', NULL, N'Active', @ImportedAt, @ImportedAt);
END

COMMIT TRANSACTION;

-- Source rows: 155. Go-Live dates matched: 149.