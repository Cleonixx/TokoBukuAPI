-- =============================================
-- DATABASE: toko_buku
-- =============================================

-- Tabel 1: categories
CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_categories_name ON categories(name);

-- Tabel 2: books
CREATE TABLE books (
    id SERIAL PRIMARY KEY,
    category_id INT NOT NULL,
    title VARCHAR(200) NOT NULL,
    author VARCHAR(100) NOT NULL,
    price DECIMAL(10,2) NOT NULL,
    stock INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_books_category FOREIGN KEY (category_id) REFERENCES categories(id)
);

CREATE INDEX idx_books_category_id ON books(category_id);
CREATE INDEX idx_books_title ON books(title);

-- Tabel 3: orders
CREATE TABLE orders (
    id SERIAL PRIMARY KEY,
    book_id INT NOT NULL,
    quantity INT NOT NULL,
    total_price DECIMAL(10,2) NOT NULL,
    customer_name VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT fk_orders_book FOREIGN KEY (book_id) REFERENCES books(id)
);

CREATE INDEX idx_orders_book_id ON orders(book_id);

-- =============================================
-- SAMPLE DATA
-- =============================================

INSERT INTO categories (name, description) VALUES
('Fiksi',        'Novel dan cerita fiksi'),
('Non-Fiksi',    'Buku pengetahuan dan fakta'),
('Teknologi',    'Buku pemrograman dan IT'),
('Bisnis',       'Buku ekonomi dan bisnis'),
('Pendidikan',   'Buku pelajaran dan akademik');

INSERT INTO books (category_id, title, author, price, stock) VALUES
(1, 'Laskar Pelangi',           'Andrea Hirata',      85000,  50),
(1, 'Bumi Manusia',             'Pramoedya Ananta',   95000,  30),
(2, 'Sapiens',                  'Yuval Noah Harari',  120000, 25),
(3, 'Clean Code',               'Robert C. Martin',   150000, 20),
(3, 'The Pragmatic Programmer', 'Andrew Hunt',        145000, 15),
(4, 'Rich Dad Poor Dad',        'Robert Kiyosaki',    90000,  40),
(5, 'Matematika Dasar',         'Soemartojo',         75000,  60);

INSERT INTO orders (book_id, quantity, total_price, customer_name) VALUES
(1, 2, 170000, 'Budi Santoso'),
(3, 1, 120000, 'Siti Rahma'),
(4, 3, 450000, 'Andi Wijaya'),
(6, 2, 180000, 'Dewi Lestari'),
(2, 1,  95000, 'Reza Pratama');
