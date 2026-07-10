-- Script Name: 001.create_hotels_table.sql
-- Script author: Juan Acevedo
-- Script date: 08/07/2026

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.tables
        WHERE table_schema = 'public'
          AND table_name = 'hotels'
    ) THEN

        CREATE TABLE public.hotels
        (
            id UUID PRIMARY KEY,

            -- General information
            name VARCHAR(200) NOT NULL,
            address VARCHAR(300) NOT NULL,
            city VARCHAR(100) NOT NULL,

            -- Status
            is_active BOOLEAN NOT NULL DEFAULT TRUE,

            -- Audit
            created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
            updated_at TIMESTAMP WITHOUT TIME ZONE,
            deleted_at TIMESTAMP WITHOUT TIME ZONE
        );

        CREATE INDEX ix_hotels_city
            ON public.hotels(city);

        CREATE INDEX ix_hotels_is_active
            ON public.hotels(is_active);

    END IF;
END $$;