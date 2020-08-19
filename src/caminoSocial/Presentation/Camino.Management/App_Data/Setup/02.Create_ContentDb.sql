CREATE TABLE dbo.Menu
(
	Id SMALLINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Url] NVARCHAR(2000) NOT NULL,
	[Description] NVARCHAR(1000),
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ParentMenuId SMALLINT NULL
)

GO
ALTER TABLE dbo.Menu
ADD CONSTRAINT PK_Menu
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.Menu
ADD CONSTRAINT FK_Menu_ParentMenu
FOREIGN KEY (ParentMenuId) REFERENCES dbo.Menu(Id);

--GROUP--
GO
CREATE TABLE dbo.[Association](
	Id BIGINT NOT NULL IDENTITY(1,1),
	Title NVARCHAR(255) NULL,
	[Description] NVARCHAR(500) NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.[Association]
ADD CONSTRAINT PK_Association
PRIMARY KEY (Id);

--GROUP ROLE--
GO
CREATE TABLE dbo.[AssociationRole]
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL
)

GO
ALTER TABLE dbo.[AssociationRole]
ADD CONSTRAINT PK_AssociationRole
PRIMARY KEY (Id);

--FARMER GROUP--
GO
CREATE TABLE dbo.AssociationMember(
	UserId BIGINT NOT NULL,
	AssociationId BIGINT NOT NULL,
	JoinedDate DATETIME2 NULL,
	IsJoined BIT NOT NULL,
	ApprovedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.AssociationMember
ADD CONSTRAINT PK_UserAssociation
PRIMARY KEY (UserId, AssociationId);

GO
ALTER TABLE dbo.AssociationMember
ADD CONSTRAINT FK_User_Association
FOREIGN KEY (AssociationId) REFERENCES dbo.[Association](Id);

--FARM--
GO
CREATE TABLE dbo.Farm
(
	Id BIGINT NOT NULL,
	Title NVARCHAR(255) NULL,
	[Description] NVARCHAR(500) NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.Farm
ADD CONSTRAINT PK_Farm
PRIMARY KEY (Id);

-- FARM ROLE --
GO
CREATE TABLE dbo.[FarmRole]
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NULL
)

GO
ALTER TABLE dbo.[FarmRole]
ADD CONSTRAINT PK_FarmRole
PRIMARY KEY (Id);
--User FARM--
GO
CREATE TABLE dbo.FarmMember(
	UserId BIGINT NOT NULL,
	FarmId BIGINT NOT NULL,
	JoinedDate DATETIME2 NULL,
	IsJoined BIT NOT NULL,
	ApprovedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.FarmMember
ADD CONSTRAINT PK_UserFarm
PRIMARY KEY (UserId, FarmId);

GO
ALTER TABLE dbo.FarmMember
ADD CONSTRAINT FK_User_Farm
FOREIGN KEY (FarmId) REFERENCES dbo.Farm(Id);

--GROUP FARM--
GO
CREATE TABLE dbo.FarmAssociation(
	AssociationId BIGINT NOT NULL,
	FarmId BIGINT NOT NULL,
	LinkedDate DATETIME2 NOT NULL,
	IsLinked BIT NOT NULL,
	LinkedById BIGINT NOT NULL,
	ApprovedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.FarmAssociation
ADD CONSTRAINT PK_FarmAssociation
PRIMARY KEY (AssociationId, FarmId);

GO
ALTER TABLE dbo.FarmAssociation
ADD CONSTRAINT FK_FarmAssociation_Association
FOREIGN KEY (AssociationId) REFERENCES dbo.[Association](Id);

GO
ALTER TABLE dbo.FarmAssociation
ADD CONSTRAINT FK_FarmAssociation_Farm
FOREIGN KEY (FarmId) REFERENCES dbo.Farm(Id);
-- CATEGORY --
GO
CREATE TABLE dbo.[ProductCategory]
(
	Id INT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ParentId INT NULL
)

GO
ALTER TABLE dbo.[ProductCategory]
ADD CONSTRAINT PK_ProductCategory
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.[ProductCategory]
ADD CONSTRAINT FK_ProductCategory_ParentProductCategory
FOREIGN KEY (ParentId) REFERENCES dbo.[ProductCategory](Id);

--PRODUCT--
GO
CREATE TABLE dbo.Product(
	Id BIGINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NOT NULL,
	Content NVARCHAR(MAX) NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.[Product]
ADD CONSTRAINT PK_Product
PRIMARY KEY (Id);

-- PRODUCT CATEGORY --
GO
CREATE TABLE dbo.ProductCategoryProduct
(
	ProductId BIGINT NOT NULL,
	ProductCategoryId INT NOT NULL
)

GO
ALTER TABLE dbo.ProductCategoryProduct
ADD CONSTRAINT FK_ProductCategoryProduct_ProductCategory
FOREIGN KEY (ProductCategoryId) REFERENCES dbo.[ProductCategory](Id);

GO
ALTER TABLE dbo.ProductCategoryProduct
ADD CONSTRAINT FK_ProductCategoryProduct_Product
FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id);

GO
ALTER TABLE dbo.ProductCategoryProduct
ADD CONSTRAINT PK_ProductCategoryProduct
PRIMARY KEY (ProductId, ProductCategoryId);

--FARM PRODUCT--
GO
CREATE TABLE dbo.FarmProduct(
	FarmId BIGINT NOT NULL,
	ProductId BIGINT NOT NULL,
	LinkedDate DATETIME2 NOT NULL,
	IsLinked BIT NOT NULL,
	LinkedById BIGINT NOT NULL,
	ApprovedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.FarmProduct
ADD CONSTRAINT PK_FarmProduct
PRIMARY KEY (FarmId, ProductId);

GO
ALTER TABLE dbo.FarmProduct
ADD CONSTRAINT FK_FarmProduct_Product
FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id);

GO
ALTER TABLE dbo.FarmProduct
ADD CONSTRAINT FK_FarmProduct_Farm
FOREIGN KEY (FarmId) REFERENCES dbo.Farm(Id);

--PRODUCT GROUP--
GO
CREATE TABLE dbo.ProductAssociation(
	AssociationId BIGINT NOT NULL,
	ProductId BIGINT NOT NULL,
	LinkedDate DATETIME2 NOT NULL,
	IsLinked BIT NOT NULL,
	LinkedById BIGINT NOT NULL,
	ApprovedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.ProductAssociation
ADD CONSTRAINT PK_ProductAssociation
PRIMARY KEY (AssociationId, ProductId);

GO
ALTER TABLE dbo.ProductAssociation
ADD CONSTRAINT FK_AssociationProduct_Product
FOREIGN KEY (ProductId) REFERENCES dbo.Product(Id);

GO
ALTER TABLE dbo.ProductAssociation
ADD CONSTRAINT FK_AssociationProduct_Association
FOREIGN KEY (AssociationId) REFERENCES dbo.[Association](Id);

-- USER PHOTO --
GO 
CREATE TABLE dbo.UserPhotoType
(
	Id TINYINT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000)
)

GO
ALTER TABLE dbo.UserPhotoType
ADD CONSTRAINT PK_UserPhotoType
PRIMARY KEY (Id);

GO
CREATE TABLE dbo.UserPhoto(
	Id BIGINT NOT NULL IDENTITY(1,1),
	Code NVARCHAR(MAX) NOT NULL,
	[Name] NVARCHAR(255) NULL,
	[Url] NVARCHAR(2000) NULL,
	[Description] NVARCHAR(1000) NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ImageData NVARCHAR(MAX) NOT NULL,
	UserId BIGINT NOT NULL,
	TypeId TINYINT NOT NULL
)

GO
ALTER TABLE dbo.UserPhoto
ADD CONSTRAINT PK_UserPhoto
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.UserPhoto
ADD CONSTRAINT FK_UserPhoto_UserPhotoType
FOREIGN KEY (TypeId) REFERENCES dbo.UserPhotoType(Id);

-- ArticleCategory --
GO
CREATE TABLE dbo.ArticleCategory
(
	Id INT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ParentId INT NULL
)

GO
ALTER TABLE dbo.ArticleCategory
ADD CONSTRAINT PK_Category
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.ArticleCategory
ADD CONSTRAINT FK_ArticleCategory_ParentCategory
FOREIGN KEY (ParentId) REFERENCES dbo.ArticleCategory(Id);

-- ARTICLE --
GO
CREATE TABLE dbo.Article
(
	Id BIGINT NOT NULL IDENTITY(1,1),
	Name NVARCHAR(255) NOT NULL,
	[Description] NVARCHAR(1000) NOT NULL,
	Content NVARCHAR(MAX) NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	ArticleCategoryId INT NOT NULL,
	Viewed INT NOT NULL
)

GO
ALTER TABLE dbo.Article
ADD CONSTRAINT PK_Article
PRIMARY KEY (Id);

GO
ALTER TABLE dbo.Article
ADD CONSTRAINT FK_Article_ArticleCategory
FOREIGN KEY (ArticleCategoryId) REFERENCES dbo.ArticleCategory(Id);

-- TAG --
GO
CREATE TABLE dbo.Tag
(
	Id INT NOT NULL IDENTITY(1,1),
	[Name] NVARCHAR(50) NOT NULL,
	CreatedDate DATETIME2 NOT NULL,
	CreatedById BIGINT NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	UpdatedById BIGINT NOT NULL
)

GO
ALTER TABLE dbo.Tag
ADD CONSTRAINT PK_Tag
PRIMARY KEY (Id);

-- ARTICLE TAG --
GO
CREATE TABLE dbo.ArticleTag
(
	ArticleId BIGINT NOT NULL,
	TagId INT NOT NULL
)

GO
ALTER TABLE dbo.ArticleTag
ADD CONSTRAINT FK_ArticleTag_Article
FOREIGN KEY (ArticleId) REFERENCES dbo.Article(Id);

GO
ALTER TABLE dbo.ArticleTag
ADD CONSTRAINT FK_ArticleTag_Tag
FOREIGN KEY (TagId) REFERENCES dbo.Tag(Id);

GO
ALTER TABLE dbo.ArticleTag
ADD CONSTRAINT PK_ArticleTag
PRIMARY KEY (ArticleId, TagId);