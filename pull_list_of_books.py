books = requests.get('https://www.goodreads.com/author/list.xml', params={'key':my_key,'id':author_id,'page':pg})
books2 = ET.fromstring(books.content)
fields=[0,4,13,14,15,19]
for book in books2[1][3]:
    x = unicode(str(author_id) + '|' + str(books2[1][1].text) + '|','utf-8','ignore')
    for f in fields:
        y = unicode(book[f].text)
        x += y +'|'
    print x