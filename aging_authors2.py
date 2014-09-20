import json
import re
import numpy as np
import time
# from lxml import etree
import requests
import xml.etree.ElementTree as ET
#tree = ET.parse('country_data.xml')
#root = tree.getroot()
import unicodedata

#get list of sci-fi authors
my_key = 'RrDHXHo9aI1Xm5CtddbA'

#ABE: This is the place where I can't get the import to work
flist = open('sci_top_100_books_partial.txt','r').read().splitlines()
#clear the old file
fout = open('sci_fi_authors_100.txt','w')
fout.close

#loop through authors
for author in flist:
    time.sleep(1) 

    author_id = 0
    
    author_response = requests.get('https://www.goodreads.com/api/author_url/'+str(author), params={'key':my_key})
    if "author id" in author_response.content:
        print author
    
        time.sleep(1)   #A small pause to obey the no more than one request per second goodreads API rule
    
        author_id = int(re.findall("<link>.*?\/show\/(\d+).*?</link>", author_response.content)[0])
    
        print author_id
    
        time.sleep(1)
    
        #get birthday
        author_show = requests.get('https://www.goodreads.com/author/show.xml', params={'key':my_key,'id':author_id})
        author_show2 = ET.fromstring(author_show.content)
        
        birthday = author_show2[1][11].text
    
        time.sleep(1)
    
        #get books, publication, and avg review
        #I need to add another loop here to go through the multiple pages for each author but I want to resolve the publication date issue first
        #I might also need to go to the book.show call to get the "original publication date" for each book, which would dramatically increase the amount of time I spent gathering the data.
        books = requests.get('https://www.goodreads.com/author/list.xml', params={'key':my_key,'id':author_id,'page':1})

#try/except
        fout = open('sci_fi_authors_100.txt','a')
        books2 = ET.fromstring(books.content)
        fields=[0,4,13,15,16,19]
        for book in books2[1][3]:
            x = unicode(author_id + '|' + books2[1][1].text + '|' + str(birthday) +'|','utf-8','ignore')
            for f in fields:
                y = unicode(book[f].text)
                x += y +'|'
            x += '\n'
            print x
            fout.write(x.encode('utf-8'))

        fout.close()
        print str(books2[1][1].text) + '|' + str(author_id) + '|' + str(birthday)
    #let me know if the wikipedia name wasn't found in goodreads
    else:
        print "No Match for " + str(author)

        
