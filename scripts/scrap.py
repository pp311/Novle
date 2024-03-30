import pyodbc
import datetime
from requests_html import HTMLSession

session = HTMLSession()
BASE = 'https://bachngocsach.com.vn/'
# Connect to the database
conn = pyodbc.connect('DRIVER={ODBC Driver 18 for SQL Server};'
                        'SERVER=127.0.0.1,1433;'
                        'DATABASE=NovleDB;'
                        'UID=sa;'
                        'PWD=Admin123;TrustServerCertificate=yes;')

cursor = conn.cursor()

# for row in cursor.tables(schema='dbo'):
#     print(row.table_name)
def execute_read_query(query, params = None):
    cursor.execute(query, params)
    return cursor.fetchall()

def execute_query(query, params = None):
    cursor.execute(query, params)
    conn.commit()

def get_chapter_info(link, bookId, chapterIndex):
    r = session.get(link)
    title = r.html.find('#chuong-title', first=True).text
    content = r.html.find('#noi-dung', first=True).text
    word_count = r.html.find('.wordcount', first=True).text.split(' ')[0]
    created_at = datetime.datetime.now()
    created_by = 1
    modified_at = datetime.datetime.now()
    modified_by = 1

    execute_query("INSERT INTO Chapter (Title, BookId, Content, WordCount, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy, [Index]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)", (title, bookId, content, word_count, created_at, created_by, modified_at, modified_by, chapterIndex))


#get book info
def get_book_info(link):
    r = session.get(link)
    try:
        title = r.html.find('#truyen-title', first=True).text
        author = r.html.find('#tacgia > a', first=True).text
        genre = r.html.find('#theloai', first=True).text
        #The first 9 characters are 'Thể loại:'
        genre_list = genre[9:].split(', ')
        description = r.html.find('#gioithieu > div.block-content', first=True).text
        flag = r.html.find('#flag', first=True).text
        status = 'ongoing'
        if (flag.find('Hoàn thành')) != -1:
            status = 'completed'
        poster = r.html.find('#anhbia > img', first=True).attrs['src']
        created_at = datetime.datetime.now()
        created_by = 1
        modified_at = datetime.datetime.now()
        modified_by = 1
        
        #insert & get author id
        row = execute_read_query("SELECT * FROM Author WHERE Name = ?", (author,))
        # print(title, author, genre_list, description, status, poster, sep='\n')
        if len(row) == 0:
            execute_query("INSERT INTO Author (Name, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy) VALUES (?, ?, ?, ?, ?)", (author, created_at, created_by, modified_at, modified_by))
        author_id = execute_read_query("SELECT Id FROM Author WHERE Name = ?", (author,))[0][0]
        
        #insert & get book id
        execute_query("INSERT INTO Book (Title, AuthorId, Description, Status, CoverUrl, CreatedOn, CreatedBy, UpdatedOn, UpdatedBy, ViewCount) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", (title, author_id, description, status, poster, created_at, created_by, modified_at, modified_by, 0))
        book_id = execute_read_query("SELECT Id FROM Book WHERE Title = ?", (title,))[0][0]

        for genre in genre_list:
            row = execute_read_query("SELECT * FROM Genre WHERE Name = ?", (genre,))
            if len(row) == 0:
                execute_query("INSERT INTO Genre (Name) VALUES (?)", (genre,))
            genre_id = execute_read_query("SELECT Id FROM Genre WHERE Name = ?", (genre,))[0][0]
            if len(execute_read_query("SELECT * FROM BookGenre WHERE BookId = ? AND GenreId = ?", (book_id, genre_id))) == 0:
                execute_query("INSERT INTO BookGenre (BookId, GenreId) VALUES (?, ?)", (book_id, genre_id))
        
        #get chapter list
        r_chapter = session.get(link + '/muc-luc?page=all')
        chapter_links = r_chapter.html.find('.chuong-link') 
        for chapter_link in chapter_links:
            print(title + ' chuong ' + str(chapter_links.index(chapter_link) + 1) )
            get_chapter_info(BASE + chapter_link.attrs['href'], book_id, chapter_links.index(chapter_link) + 1)
    except Exception as e:
        print(e)
        return

for i in range (0,12):
    result = session.get('https://bachngocsach.com.vn/reader/recent-bns?page=' + str(i))
    book_links = result.html.find('.recent-anhbia-a')
    for book_link in book_links:
        print(book_link.attrs['href'])
        get_book_info(BASE + book_link.attrs['href'])


