create trigger t1 on  HistoriaChorobies
after insert, update
AS
 begin
	update HistoriaChorobies 
	set OstatniaModyfikacjaOpisuChoroby = GETDATE()
	where HistoriaChorobies.Pesel in (select distinct Pesel from inserted)
	and HistoriaChorobies.IdLekarza in (select distinct IdLekarza from inserted)
 end
 go
create view LiczbaPacjentowLekarza as
select l.Imie,l.Nazwisko, count(p.Pesel) as LiczbaPacjentow from Pacjents p 
join HistoriaChorobies h
on h.Pesel like p.Pesel
join Lekarzs l
on h.IdLekarza like l.IdLekarza
group by l.Imie,l.Nazwisko

select * from LiczbaPacjentowLekarza
select * from HistoriaChorobies