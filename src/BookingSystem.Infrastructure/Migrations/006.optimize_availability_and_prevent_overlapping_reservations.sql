-- Script Name: 006.optimize_availability_and_prevent_overlapping_reservations.sql
-- Purpose:
--   1. Improve room availability filtering with partial indexes.
--   2. Enforce the no-overlapping-reservations rule at database level.
--
-- IMPORTANT:
--   Review and resolve existing overlapping active reservations before applying
--   this script. The exclusion constraint intentionally fails if conflicting
--   data already exists.
--   The database role must be allowed to install the btree_gist extension.

CREATE EXTENSION IF NOT EXISTS btree_gist;

-- Partial index for active hotels searched by city.
CREATE INDEX IF NOT EXISTS ix_hotels_city_active_not_deleted
    ON public.hotels (city)
    WHERE is_active = TRUE
      AND deleted_at IS NULL;

-- Supports active-room filtering by hotel and minimum guest capacity.
CREATE INDEX IF NOT EXISTS ix_rooms_hotel_capacity_active_not_deleted
    ON public.rooms (hotel_id, capacity)
    WHERE is_active = TRUE
      AND deleted_at IS NULL;

-- Prevent two non-cancelled reservations from overlapping for the same room.
-- [) means check-in is inclusive and check-out is exclusive, so a guest can
-- check out on the same date/time another guest checks in.
DO $$
BEGIN
    IF NOT EXISTS (
        SELECT 1
        FROM pg_constraint
        WHERE conname = 'ex_reservations_room_date_overlap'
          AND conrelid = 'public.reservations'::regclass
    ) THEN
        ALTER TABLE public.reservations
            ADD CONSTRAINT ex_reservations_room_date_overlap
            EXCLUDE USING gist (
                room_id WITH =,
                tsrange(check_in, check_out, '[)') WITH &&
            )
            WHERE (status IN (1, 2) AND deleted_at IS NULL);
    END IF;
END $$;
