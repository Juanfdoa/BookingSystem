-- Script Name: 003.create_reservations_table.sql
-- Script author: Juan Acevedo
-- Script date: 08/07/2026

DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM information_schema.tables
        WHERE table_schema = 'public'
          AND table_name = 'reservations'
    ) THEN

        CREATE TABLE public.reservations
        (
            id UUID PRIMARY KEY,

            -- Relationships
            room_id UUID NOT NULL,

            -- Reservation information
            check_in TIMESTAMP WITHOUT TIME ZONE NOT NULL,
            check_out TIMESTAMP WITHOUT TIME ZONE NOT NULL,
            status INTEGER NOT NULL,
            total_cost NUMERIC(12,2) NOT NULL,

            -- Audit
            created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
            updated_at TIMESTAMP WITHOUT TIME ZONE,
            deleted_at TIMESTAMP WITHOUT TIME ZONE,

            CONSTRAINT fk_reservations_rooms
                FOREIGN KEY (room_id)
                REFERENCES public.rooms(id),

            CONSTRAINT ck_reservations_dates
                CHECK (check_out > check_in),

            CONSTRAINT ck_reservations_total_cost
                CHECK (total_cost >= 0)
        );

        CREATE INDEX ix_reservations_room
            ON public.reservations(room_id);

        CREATE INDEX ix_reservations_status
            ON public.reservations(status);

        CREATE INDEX ix_reservations_check_in
            ON public.reservations(check_in);

        CREATE INDEX ix_reservations_check_out
            ON public.reservations(check_out);

    END IF;
END $$;