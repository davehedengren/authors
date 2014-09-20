import json
import re
import time
import numpy as np
# from lxml import etree
import requests
import xml.etree.ElementTree as ET
#tree = ET.parse('country_data.xml')
#root = tree.getroot()

#get list of sci-fi authors
sci_fi_authors = requests.get('http://en.wikipedia.org/wiki/List_of_science_fiction_authors')

f = open("sci_fi_author_get.txt","w")
f.write(sci_fi_authors.content)
#restrict the html to just the names and dates
authors = open("sf.txt","w")
for line in sci_fi_authors:
    if re.search('\</dd\>', line):
        authors.write(line)

#make list of names and dates
#split delim <>;
#index or find on an array
authors = open("sf.txt","r")
authors2 = authors.readlines()
#remove junk to the left
authors3 = ([re.sub('.*?(title=)', '',s , 1) for s in authors2])
#remove junk to the right
authors4 = ([re.sub('>.*', '',s , 1) for s in authors3])
authors5 = ([re.sub(' class.*', '',s , 1) for s in authors4])
authors6 = ([re.sub(' \(.*\)', '',s , 1) for s in authors5])


authors_names = open("sf2.txt","w")
authors_names.writelines(authors6)


np.savetxt("sf2.txt",authors5)
authors_names.write(authors5)


my_key = 'RrDHXHo9aI1Xm5CtddbA'

#get names

author_id = 0
author = 'Isaac Asimov'
author_response = requests.get('https://www.goodreads.com/api/author_url/'+str(author), params={'key':my_key})

author_id = int(re.findall("<link>.*?\/show\/(\d+).*?</link>", author_response.content)[0])
if author_id > 0:
    print author_id
#get birthday
author_show = requests.get('https://www.goodreads.com/author/show.xml', params={'key':my_key,'id':author_id})
author_show2 = ET.fromstring(author_show.content)
birthday = author_show2[1][11].text

#get list of books, publication date, and reviews
books = requests.get('https://www.goodreads.com/author/list.xml', params={'key':my_key,'id':author_id,'page':1})

fout = open('auth_'+str(author_id)+'.txt','w')
books2 = ET.fromstring(books.content)
fields=[0,4,13,14,15,19]
for book in books2[1][3]:
    x = str(author_id) + '|' + books2[1][1].text + birthday +'|'
    for f in fields:
        x += str(book[f].text)+'|'
    x += '\n'
    fout.write(x)

fout.close()
   
else:
    print "No Match for " + str(author)
    



for l in author_show2[1]:
    print l.text
    


fin = open('auth_'+str(author_id)+'.txt','r')
lines=fin.readlines()
for line in lines:
    print line.split('|')[3]




sci_fi_authors2 = 

a_file = open("sci_fi_list.txt","w")
a_file.write(print(sci_fi_authors.content))
file.close
a_line = ['href="/wiki/']

with open('oldfile.txt') as oldfile, open('newfile.txt', 'w') as newfile:
    for line in oldfile:
        if any(a_line in line for bad_word in bad_words):
            newfile.write(line)
            
            

    if re.search('[0-9]{4}-[0-9]{4}', line):
        a2 = line
        dead.append(a2)
        
        
#restrict the html to just the names and dates
authors = open("sf.txt","w")
for line in sci_fi_authors:
    if re.search('\</dd\>', line):
        authors.write(line)
        

for (i,z) in zip(child,range(len(child))):
    print str(z)+'\t'+str(i)
    
    [0,4,13,16,15,19]
        0	<Element 'id' at 0x10940ab90>
        1	<Element 'isbn' at 0x10940abd0>
        2	<Element 'isbn13' at 0x10940ac10>
        3	<Element 'text_reviews_count' at 0x10940ac50>
        4	<Element 'title' at 0x10940ac90>
        5	<Element 'image_url' at 0x10940ad10>
        6	<Element 'small_image_url' at 0x10940ad50>
        7	<Element 'link' at 0x10940ad90>
        8	<Element 'num_pages' at 0x10940add0>
        9	<Element 'format' at 0x10940ae10>
        10	<Element 'edition_information' at 0x10940ae50>
        11	<Element 'publisher' at 0x10940ae90>
        12	<Element 'publication_day' at 0x10940aed0>
        13	<Element 'publication_year' at 0x10940af10>
        14	<Element 'publication_month' at 0x10940af50>
        15	<Element 'average_rating' at 0x10940af90>
        16	<Element 'ratings_count' at 0x10940afd0>
        17	<Element 'description' at 0x10940e050>
        18	<Element 'authors' at 0x10940e090>
        19	<Element 'published' at 0x10940e310>
        
        
        
        
            x = book[18].text + '|' + book[0].text + '|' + book[4].text + '|' + book[13].text + '|' + book[15].text


#from abe
#Look up Charles Dickens
author_response = requests.get('https://www.goodreads.com/api/author_url/Charles%20Dickens', params={'key':my_key})

author_id = int(re.findall("<link>.*?\/show\/(\d+).*?</link>", author_response.content)[0])

#Fetch the first page of his books
books = resquests.get('https://www.goodreads.com/author/list.xml', params={'key':my_key,'id':author_id,'page':1})
books_response1 = requests.get('https://www.goodreads.com/author/list.xml?key=RrDHXHo9aI1Xm5CtddbA&id=239579'+str(author_id)+'.json', params={'key':my_key, 'page':1})
print books_response1.content

#Fetch the second page of his books...
books_response2 = requests.get('https://www.goodreads.com/author/list/'+str(author_id)+'.json', params={'key':my_key, 'page':2})
print books_response.content

