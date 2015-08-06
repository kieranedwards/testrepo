
https://github.com/schambers/fluentmigrator/wiki/Migration

##SQL Rules##

All SQL to be gernated via this migration package

Table Names Non Plurualised - http://stackoverflow.com/a/5841297/33

Id Columns Suffixed Id - OrderId
Other Columns not - Price not OrderPrice

Date columns shoudl start with the word date:

DateCreated
DateUdpated

Alway set description so DB schema doc can be produced


Migration files created in format  yyyyMMdd


All SQL in SP's Named - [PrimaryObject]_[Action][Process]