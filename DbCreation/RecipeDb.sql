/****** Object:  Table [dbo].[AmountType]    Script Date: 22-05-2023 13:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AmountType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DateCreate] [datetime] NOT NULL,
	[DateChange] [datetime] NULL,
 CONSTRAINT [PK_AmountType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 22-05-2023 13:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DateCreate] [datetime] NOT NULL,
	[DateChange] [datetime] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ingredient]    Script Date: 22-05-2023 13:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredient](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DateCreate] [datetime] NOT NULL,
	[DateChange] [datetime] NULL,
 CONSTRAINT [PK_Ingredient] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipe]    Script Date: 22-05-2023 13:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipe](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Votes] [int] NOT NULL,
	[DateCreate] [datetime] NOT NULL,
	[DateChange] [datetime] NULL,
 CONSTRAINT [PK_Recipes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecipeIngredient]    Script Date: 22-05-2023 13:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecipeIngredient](
	[RecipeId] [int] NOT NULL,
	[IngredientId] [int] NOT NULL,
	[AmountTypeId] [int] NOT NULL,
	[Amount] [int] NOT NULL,
	[DateCreate] [datetime] NOT NULL,
	[DateChange] [datetime] NULL,
 CONSTRAINT [PK_RecipeIngredient] PRIMARY KEY CLUSTERED 
(
	[RecipeId] ASC,
	[IngredientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AmountType] ON 

INSERT [dbo].[AmountType] ([Id], [Name], [DateCreate], [DateChange]) VALUES (1, N'gr', CAST(N'2023-05-22T02:10:03.203' AS DateTime), NULL)
INSERT [dbo].[AmountType] ([Id], [Name], [DateCreate], [DateChange]) VALUES (2, N'kg', CAST(N'2023-05-22T02:10:08.737' AS DateTime), NULL)
INSERT [dbo].[AmountType] ([Id], [Name], [DateCreate], [DateChange]) VALUES (3, N'tsp', CAST(N'2023-05-22T02:11:01.033' AS DateTime), NULL)
INSERT [dbo].[AmountType] ([Id], [Name], [DateCreate], [DateChange]) VALUES (4, N'stk', CAST(N'2023-05-22T02:12:22.907' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[AmountType] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([Id], [Name], [DateCreate], [DateChange]) VALUES (1, N'Morgenmad', CAST(N'2023-05-21T14:00:51.047' AS DateTime), NULL)
INSERT [dbo].[Category] ([Id], [Name], [DateCreate], [DateChange]) VALUES (2, N'Frokost', CAST(N'2023-05-21T14:01:25.800' AS DateTime), NULL)
INSERT [dbo].[Category] ([Id], [Name], [DateCreate], [DateChange]) VALUES (3, N'Aftensmad', CAST(N'2023-05-21T14:01:27.950' AS DateTime), NULL)
INSERT [dbo].[Category] ([Id], [Name], [DateCreate], [DateChange]) VALUES (4, N'Dessert', CAST(N'2023-05-21T14:01:47.490' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Ingredient] ON 

INSERT [dbo].[Ingredient] ([Id], [Name], [DateCreate], [DateChange]) VALUES (1, N'Æg', CAST(N'2023-05-22T02:10:25.970' AS DateTime), NULL)
INSERT [dbo].[Ingredient] ([Id], [Name], [DateCreate], [DateChange]) VALUES (2, N'Fløde', CAST(N'2023-05-22T02:10:30.387' AS DateTime), NULL)
INSERT [dbo].[Ingredient] ([Id], [Name], [DateCreate], [DateChange]) VALUES (3, N'Salt', CAST(N'2023-05-22T02:10:38.307' AS DateTime), NULL)
INSERT [dbo].[Ingredient] ([Id], [Name], [DateCreate], [DateChange]) VALUES (4, N'Peber', CAST(N'2023-05-22T02:10:40.823' AS DateTime), NULL)
INSERT [dbo].[Ingredient] ([Id], [Name], [DateCreate], [DateChange]) VALUES (5, N'Forårsløg', CAST(N'2023-05-22T02:10:45.233' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Ingredient] OFF
GO
SET IDENTITY_INSERT [dbo].[Recipe] ON 

INSERT [dbo].[Recipe] ([Id], [CategoryId], [Name], [Description], [Votes], [DateCreate], [DateChange]) VALUES (1, 1, N'Æggekage', N'Lækker lækker ', 6, CAST(N'2023-05-22T02:11:52.160' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Recipe] OFF
GO
INSERT [dbo].[RecipeIngredient] ([RecipeId], [IngredientId], [AmountTypeId], [Amount], [DateCreate], [DateChange]) VALUES (1, 1, 1, 4, CAST(N'2023-05-22T02:12:05.577' AS DateTime), NULL)
INSERT [dbo].[RecipeIngredient] ([RecipeId], [IngredientId], [AmountTypeId], [Amount], [DateCreate], [DateChange]) VALUES (1, 2, 3, 4, CAST(N'2023-05-22T02:13:23.690' AS DateTime), NULL)
INSERT [dbo].[RecipeIngredient] ([RecipeId], [IngredientId], [AmountTypeId], [Amount], [DateCreate], [DateChange]) VALUES (1, 3, 1, 5, CAST(N'2023-05-22T02:14:13.020' AS DateTime), NULL)
INSERT [dbo].[RecipeIngredient] ([RecipeId], [IngredientId], [AmountTypeId], [Amount], [DateCreate], [DateChange]) VALUES (1, 4, 1, 5, CAST(N'2023-05-22T02:14:34.063' AS DateTime), NULL)
INSERT [dbo].[RecipeIngredient] ([RecipeId], [IngredientId], [AmountTypeId], [Amount], [DateCreate], [DateChange]) VALUES (1, 5, 4, 2, CAST(N'2023-05-22T02:13:47.470' AS DateTime), NULL)
GO
ALTER TABLE [dbo].[AmountType] ADD  CONSTRAINT [DF_AmountType_DateCreate]  DEFAULT (getdate()) FOR [DateCreate]
GO
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Category_DateCreate]  DEFAULT (getdate()) FOR [DateCreate]
GO
ALTER TABLE [dbo].[Ingredient] ADD  CONSTRAINT [DF_Ingredient_DateCreate]  DEFAULT (getdate()) FOR [DateCreate]
GO
ALTER TABLE [dbo].[Recipe] ADD  CONSTRAINT [DF_Recipe_Vote]  DEFAULT ((0)) FOR [Votes]
GO
ALTER TABLE [dbo].[Recipe] ADD  CONSTRAINT [DF_Recipes_DateCreate]  DEFAULT (getdate()) FOR [DateCreate]
GO
ALTER TABLE [dbo].[RecipeIngredient] ADD  CONSTRAINT [DF_RecipeIngredient_DateCreate]  DEFAULT (getdate()) FOR [DateCreate]
GO
ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Recipe] CHECK CONSTRAINT [FK_Recipe_Category]
GO
ALTER TABLE [dbo].[RecipeIngredient]  WITH CHECK ADD  CONSTRAINT [FK_RecipeIngredient_AmountType] FOREIGN KEY([AmountTypeId])
REFERENCES [dbo].[AmountType] ([Id])
GO
ALTER TABLE [dbo].[RecipeIngredient] CHECK CONSTRAINT [FK_RecipeIngredient_AmountType]
GO
ALTER TABLE [dbo].[RecipeIngredient]  WITH CHECK ADD  CONSTRAINT [FK_RecipeIngredient_Ingredient] FOREIGN KEY([IngredientId])
REFERENCES [dbo].[Ingredient] ([Id])
GO
ALTER TABLE [dbo].[RecipeIngredient] CHECK CONSTRAINT [FK_RecipeIngredient_Ingredient]
GO
ALTER TABLE [dbo].[RecipeIngredient]  WITH CHECK ADD  CONSTRAINT [FK_RecipeIngredient_Recipe] FOREIGN KEY([RecipeId])
REFERENCES [dbo].[Recipe] ([Id])
GO
ALTER TABLE [dbo].[RecipeIngredient] CHECK CONSTRAINT [FK_RecipeIngredient_Recipe]
GO
