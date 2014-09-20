clear all
cd "/Users/davidhedengren/Dropbox/Blog/Do_Authors_Age_Well/"
import delimited "/Users/davidhedengren/Dropbox/Blog/Do_Authors_Age_Well/sci_fi_authors.csv"

gen age = publication-birthyear
drop if age < 10
drop if age > 80

sort auth_id 

save sf, replace

collapse (min) first_pub=publication (count) n_pub=publication, by(auth_id)

merge 1:m auth_id using sf

gen exp = publication-first_pub

reg avg_review age if n_pub >= 5, r 
reg avg_review exp if n_pub >= 5, r 
