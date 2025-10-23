UPDATE Products 
SET ImagePath = 'images/laptop.svg', 
    Description = 'Мощен лаптоп за работа и забавление',
    Category = 'Електроника',
    InStock = 1
WHERE Name LIKE '%Laptop%';
