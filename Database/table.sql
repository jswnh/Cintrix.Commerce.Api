CREATE DATABASE IF NOT EXISTS cintrix_db;

USE cintrix_db;

CREATE TABLE IF NOT EXISTS items (
    item_id CHAR(36) NOT NULL UNIQUE,
    label VARCHAR(250) NOT NULL,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP DEFAULT NULL,
    PRIMARY KEY (item_id)
);

CREATE TABLE IF NOT EXISTS item_variants (
    item_variant_id CHAR(36) NOT NULL UNIQUE,
    item_id CHAR(36) NOT NULL,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP DEFAULT NULL,
    PRIMARY KEY (item_variant_id),
    CONSTRAINT fb_item_variants_items FOREIGN KEY (item_id) REFERENCES items(item_id)
);

CREATE TABLE IF NOT EXISTS categories (
    category_id CHAR(36) NOT NULL UNIQUE,
    label VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP DEFAULT NULL
);

CREATE TABLE IF NOT EXISTS item_categories (
    item_category_id CHAR(36) NOT NULL UNIQUE,
    category_id CHAR(36) NOT NULL,
    item_id CHAR(36) NOT NULL,
    PRIMARY KEY (item_category_id),
    UNIQUE KEY uw_item_category (category_id,item_id),
    CONSTRAINT fk_item_categories_categories FOREIGN KEY (category_id) REFERENCES categories(category_id),
    CONSTRAINT fk_item_categories_items FOREIGN KEY (item_id) REFERENCES items(item_id)
);

CREATE TABLE IF NOT EXISTS brands (
    brand_id CHAR(36) NOT NULL UNIQUE,
    label VARCHAR(100) NOT NULL UNIQUE,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP DEFAULT NULL,
    PRIMARY KEY (brand_id)
);