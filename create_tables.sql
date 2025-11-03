-- FarmToTable Database Schema
-- Creates all required tables for the application

-- Farmers table (must be created first as it's referenced by other tables)
CREATE TABLE IF NOT EXISTS farmers (
    farmer_id INT AUTO_INCREMENT PRIMARY KEY,
    farm_name VARCHAR(255) NOT NULL,
    first_name VARCHAR(255) NOT NULL,
    last_name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    phone VARCHAR(50),
    address VARCHAR(500),
    city VARCHAR(100),
    state VARCHAR(50),
    zip_code VARCHAR(20),
    bank_account_last4 VARCHAR(4),
    bank_account_token VARCHAR(255),
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_email (email)
);

-- Schools table (must be created before orders table)
CREATE TABLE IF NOT EXISTS schools (
    school_id INT AUTO_INCREMENT PRIMARY KEY,
    school_name VARCHAR(255) NOT NULL,
    contact_name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    phone VARCHAR(50),
    address VARCHAR(500),
    city VARCHAR(100),
    state VARCHAR(50),
    zip_code VARCHAR(20),
    credit_card_last4 VARCHAR(4),
    credit_card_token VARCHAR(255),
    is_verified BOOLEAN DEFAULT FALSE,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    INDEX idx_email (email)
);

-- Produce table
-- Use IF NOT EXISTS to avoid errors if table already exists
-- If table exists with wrong structure, use alter_produce_table.sql to fix it
CREATE TABLE IF NOT EXISTS produce (
    produce_id INT AUTO_INCREMENT PRIMARY KEY,
    farmer_id INT NOT NULL,
    produce_name VARCHAR(255) NOT NULL,
    description TEXT,
    price_per_unit DECIMAL(10, 2) NOT NULL,
    available_quantity INT NOT NULL DEFAULT 0,
    unit VARCHAR(50) NOT NULL,
    farmer_can_deliver BOOLEAN DEFAULT FALSE,
    availability_start DATE NOT NULL,
    availability_end DATE NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (farmer_id) REFERENCES farmers(farmer_id) ON DELETE CASCADE,
    INDEX idx_farmer_id (farmer_id),
    INDEX idx_availability (availability_start, availability_end)
);

-- Orders table
CREATE TABLE IF NOT EXISTS orders (
    order_id INT AUTO_INCREMENT PRIMARY KEY,
    school_id INT NOT NULL,
    farmer_id INT NOT NULL,
    order_date DATETIME NOT NULL,
    delivery_date DATETIME NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'Pending',
    total_cost DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    payment_status VARCHAR(50) NOT NULL DEFAULT 'Unpaid',
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (school_id) REFERENCES schools(school_id) ON DELETE CASCADE,
    FOREIGN KEY (farmer_id) REFERENCES farmers(farmer_id) ON DELETE CASCADE,
    INDEX idx_school_id (school_id),
    INDEX idx_farmer_id (farmer_id),
    INDEX idx_order_date (order_date),
    INDEX idx_status (status)
);

-- Order Items table
CREATE TABLE IF NOT EXISTS order_items (
    order_item_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    produce_id INT NOT NULL,
    quantity INT NOT NULL,
    unit_price DECIMAL(10, 2) NOT NULL,
    subtotal DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE,
    FOREIGN KEY (produce_id) REFERENCES produce(produce_id) ON DELETE CASCADE,
    INDEX idx_order_id (order_id),
    INDEX idx_produce_id (produce_id)
);

-- Deliveries table
CREATE TABLE IF NOT EXISTS deliveries (
    delivery_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    truck_company VARCHAR(255),
    truck_contact VARCHAR(255),
    delivery_fee_total DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    school_share DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    farmer_share DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    delivery_status VARCHAR(50) NOT NULL DEFAULT 'Scheduled',
    estimated_arrival DATETIME NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE,
    INDEX idx_order_id (order_id),
    INDEX idx_delivery_status (delivery_status)
);

-- Payments table
CREATE TABLE IF NOT EXISTS payments (
    payment_id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT NOT NULL,
    amount DECIMAL(10, 2) NOT NULL,
    payment_method VARCHAR(100) NOT NULL,
    transaction_id VARCHAR(255),
    payment_date DATETIME NOT NULL,
    status VARCHAR(50) NOT NULL DEFAULT 'Pending',
    FOREIGN KEY (order_id) REFERENCES orders(order_id) ON DELETE CASCADE,
    INDEX idx_order_id (order_id),
    INDEX idx_payment_date (payment_date),
    INDEX idx_status (status)
);

-- System Logs table (optional but defined in context.md)
CREATE TABLE IF NOT EXISTS system_logs (
    log_id INT AUTO_INCREMENT PRIMARY KEY,
    user_email VARCHAR(255) NOT NULL,
    action VARCHAR(255) NOT NULL,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    ip_address VARCHAR(45),
    INDEX idx_user_email (user_email),
    INDEX idx_timestamp (timestamp)
);

