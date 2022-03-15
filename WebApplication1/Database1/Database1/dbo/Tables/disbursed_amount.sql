CREATE TABLE [dbo].[disbursed_amount] (
    [Id]     INT           NOT NULL,
    [month]  NVARCHAR (10) NULL,
    [amount] NVARCHAR (10) NULL,
    [number] NVARCHAR (10) NULL,
    [remark] NVARCHAR (20) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

