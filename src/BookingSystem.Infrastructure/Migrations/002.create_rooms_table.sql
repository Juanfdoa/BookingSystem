-- Script Name: 002.create_rooms_table.sql
-- Script author: Juan Acevedo
-- Script date: 08/07/2026

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.tables
        WHERE table_schema = 'public'
          AND table_name = 'rooms'
    ) THEN

        CREATE TABLE public.rooms
        (
            id UUID PRIMARY KEY,

            -- Relationships
            hotel_id UUID NOT NULL,

            -- Room information
            number VARCHAR(20) NOT NULL,
            type INTEGER NOT NULL,
            location VARCHAR(100) NOT NULL,
            capacity INTEGER NOT NULL,
            base_price NUMERIC(12,2) NOT NULL,
            taxes NUMERIC(12,2) NOT NULL DEFAULT 0,

            -- Status
            is_active BOOLEAN NOT NULL DEFAULT TRUE,

            -- Audit
            created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
            updated_at TIMESTAMP WITHOUT TIME ZONE,
            deleted_at TIMESTAMP WITHOUT TIME ZONE,

            CONSTRAINT fk_rooms_hotels
                FOREIGN KEY (hotel_id)
                REFERENCES public.hotels(id),

            CONSTRAINT ck_rooms_capacity
                CHECK (capacity > 0),

            CONSTRAINT ck_rooms_base_price
                CHECK (base_price >= 0),

            CONSTRAINT ck_rooms_taxes
                CHECK (taxes >= 0)
        );

        CREATE INDEX ix_rooms_hotel
            ON public.rooms(hotel_id);

        CREATE INDEX ix_rooms_hotel_active
            ON public.rooms(hotel_id, is_active);

        CREATE INDEX ix_rooms_type
            ON public.rooms(type);

        CREATE INDEX ix_rooms_active
            ON public.rooms(is_active);

    END IF;
END $$;