INSERT INTO [dbo].[Category] ([CategoryItem])
VALUES
('Electronics'),
('Footwear'),
('Fashion');

INSERT INTO [dbo].[Product] ([Name], [Price], [Stock], [Unit], [Category])
VALUES
('iPhone 13 Pro', 1099, 50, 1, 'Electronics'),
('Samsung Galaxy S21', 899, 100, 1, 'Electronics'),
('Sony PlayStation 5', 499, 25, 1, 'Electronics'),
('Canon EOS R5', 3899, 10, 1, 'Electronics'),
('Nintendo Switch', 299, 200, 1, 'Electronics'),
('Nike Air Force 1', 100, 150, 1, 'Footwear'),
('Adidas UltraBoost', 180, 75, 1, 'Footwear'),
('Gucci GG Marmont Bag', 1980, 30, 1, 'Fashion'),
('Rolex Submariner', 9000, 5, 1, 'Fashion'),
('Hermes Birkin Bag', 12000, 10, 1, 'Fashion'),
('MacBook Pro', 2399, 20, 1, 'Electronics'),
('Dell XPS 15', 1599, 15, 1, 'Electronics'),
('Samsung QLED 4K TV', 1499, 30, 1, 'Electronics'),
('Sony WH-1000XM4 Headphones', 349, 50, 1, 'Electronics'),
('Canon EOS 5D Mark IV', 2499, 5, 1, 'Electronics'),
('Bose QuietComfort Earbuds', 279, 40, 1, 'Electronics'),
('Apple Watch Series 7', 399, 70, 1, 'Electronics'),
('Nike Air Jordan 1', 170, 100, 1, 'Footwear'),
('Adidas Stan Smith', 80, 200, 1, 'Footwear'),
('Louis Vuitton Speedy Bag', 1200, 20, 1, 'Fashion'),
('Chanel Classic Flap Bag', 6000, 15, 1, 'Fashion'),
('HP Spectre x360', 1299, 25, 1, 'Electronics'),
('Lenovo ThinkPad X1 Carbon', 1699, 20, 1, 'Electronics'),
('LG OLED 4K TV', 1999, 35, 1, 'Electronics'),
('JBL Flip 5 Bluetooth Speaker', 99, 60, 1, 'Electronics'),
('Nikon D850', 3299, 10, 1, 'Electronics'),
('Sennheiser HD 660 S Headphones', 499, 30, 1, 'Electronics'),
('Fitbit Versa 3', 229, 90, 1, 'Electronics'),
('Puma RS-X3', 120, 150, 1, 'Footwear'),
('New Balance 990v5', 175, 100, 1, 'Footwear');