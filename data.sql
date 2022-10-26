INSERT INTO testdb.public.user ("Id", "Email", "Name", "Role")
VALUES ('88d1bef4-3a48-4476-8b20-2662cb795c9a', 'lars@hvl.no', 'Lars', 0);


INSERT INTO testdb.public.poll ("Id", "Pincode", "Question", "OptionOne", "OptionTwo", "Access", "StartTime", "EndTime", "CreatedTime", "OwnerId")
VALUES ('366d6eae-8280-447f-a555-623306b08580', '123456', 'Ananas på pizza?', 'Ja', 'Nei!', 0, current_date, current_date, current_date, '88d1bef4-3a48-4476-8b20-2662cb795c9a');

INSERT INTO testdb.public.vote ("Id", "OptionSelected", "UserId", "PollId")
VALUES ('50dcaf76-4673-408d-8647-11949a9654b3', 0, '88d1bef4-3a48-4476-8b20-2662cb795c9a', '366d6eae-8280-447f-a555-623306b08580');