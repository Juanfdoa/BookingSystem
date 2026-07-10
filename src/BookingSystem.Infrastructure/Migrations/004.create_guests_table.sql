-- Script Name: 004.create_guests_table.sql
-- Script author: Juan Acevedo
-- Script date: 08/07/2026

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.tables
        WHERE table_schema = 'public'
          AND table_name = 'guests'
    ) THEN

        CREATE TABLE public.guests
        (
            id UUID PRIMARY KEY,

            -- Relationships
            reservation_id UUID NOT NULL,

            -- Guest information
            first_name VARCHAR(100) NOT NULL,
            last_name VARCHAR(100) NOT NULL,
            gender INTEGER NOT NULL,
            document_type INTEGER NOT NULL,
            document_number VARCHAR(50) NOT NULL,
            email VARCHAR(200) NOT NULL,
            phone VARCHAR(30) NOT NULL,

            -- Audit
            created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
            updated_at TIMESTAMP WITHOUT TIME ZONE,
            deleted_at TIMESTAMP WITHOUT TIME ZONE,

            CONSTRAINT fk_guests_reservations
                FOREIGN KEY (reservation_id)
                REFERENCES public.reservations(id)
        );

        CREATE INDEX ix_guests_reservation
            ON public.guests(reservation_id);

        CREATE INDEX ix_guests_document
            ON public.guests(document_number);

        CREATE INDEX ix_guests_email
            ON public.guests(email);

    END IF;
END $$;