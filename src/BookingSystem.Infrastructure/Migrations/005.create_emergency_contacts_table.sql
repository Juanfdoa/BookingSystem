-- Script Name: 005.create_emergency_contacts_table.sql
-- Script author: Juan Acevedo
-- Script date: 08/07/2026

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.tables
        WHERE table_schema = 'public'
          AND table_name = 'emergency_contacts'
    ) THEN

        CREATE TABLE public.emergency_contacts
        (
            id UUID PRIMARY KEY,

            -- Relationship
            reservation_id UUID NOT NULL,

            -- Contact information
            first_name VARCHAR(100) NOT NULL,
            last_name VARCHAR(100) NOT NULL,
            phone VARCHAR(30) NOT NULL,

            -- Audit
            created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
            updated_at TIMESTAMP WITHOUT TIME ZONE,
            deleted_at TIMESTAMP WITHOUT TIME ZONE,

            CONSTRAINT fk_emergency_contacts_reservations
                FOREIGN KEY (reservation_id)
                REFERENCES public.reservations(id),

            CONSTRAINT uq_emergency_contact_reservation
                UNIQUE (reservation_id)
        );

        CREATE INDEX ix_emergency_contacts_reservation
            ON public.emergency_contacts(reservation_id);

    END IF;
END $$;