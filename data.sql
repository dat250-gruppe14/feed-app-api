INSERT INTO testdb.public.user ("Id", "Email", "Name", "Role", "PasswordHash", "PasswordSalt")
VALUES ('88d1bef4-3a48-4476-8b20-2662cb795c9a', 'lars@hvl.no', 'Lars', 0, 'DF9KBOMDKFm0QUgNwK4XJA==', '$2a$11$sF/Yc82MSTrcTQxbIxxLRe');
-- password = asd123


INSERT INTO testdb.public.poll ("Id", "Pincode", "Question", "OptionOne", "OptionTwo", "Access", "StartTime", "EndTime", "CreatedTime", "OwnerId")
VALUES ('366d6eae-8280-447f-a555-623306b08580', '123456', 'Ananas på pizza?', 'Ja', 'Nei!', 0, current_date, current_date, current_date, '88d1bef4-3a48-4476-8b20-2662cb795c9a');

INSERT INTO testdb.public.vote ("Id", "OptionSelected", "PollId")
VALUES ('50dcaf76-4673-408d-8647-11949a9654b3', 0, '366d6eae-8280-447f-a555-623306b08580');