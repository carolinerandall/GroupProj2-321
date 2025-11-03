-- Fix produce table structure
-- This script ensures the produce table has the correct column names

-- First, check if we need to alter or recreate the table
-- If the table exists but has wrong columns, drop and recreate

DROP TABLE IF EXISTS produce;

CREATE TABLE produce (
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

