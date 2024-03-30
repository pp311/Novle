import pyodbc
import datetime
from requests_html import HTMLSession

session = HTMLSession()
BASE = 'https://bachngocsach.com.vn/'
# Connect to the database
conn = pyodbc.connect('DRIVER={ODBC Driver 18 for SQL Server};'
                        'SERVER=localhost,1433;'
                        'DATABASE=WebTruyenChuDB;'
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
    created_by = 'pp311'
    modified_at = datetime.datetime.now()
    modified_by = 'pp311'

    execute_query("INSERT INTO Chapters (ChapterName, BookID, Content, WordCount, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, ChapterIndex) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)", (title, bookId, content, word_count, created_at, created_by, modified_at, modified_by, chapterIndex))


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
        created_by = 'pp311'
        modified_at = datetime.datetime.now()
        modified_by = 'pp311'
        
        #insert & get author id
        row = execute_read_query("SELECT * FROM Authors WHERE AuthorName = ?", (author,))
        # print(title, author, genre_list, description, status, poster, sep='\n')
        if len(row) == 0:
            execute_query("INSERT INTO Authors (AuthorName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy) VALUES (?, ?, ?, ?, ?)", (author, created_at, created_by, modified_at, modified_by))
        author_id = execute_read_query("SELECT AuthorID FROM Authors WHERE AuthorName = ?", (author,))[0][0]
        
        #insert & get book id
        execute_query("INSERT INTO Books (BookName, AuthorID, Description, Status, PosterUrl, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy, ViewCount) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", (title, author_id, description, status, poster, created_at, created_by, modified_at, modified_by, 0))
        book_id = execute_read_query("SELECT BookID FROM Books WHERE BookName = ?", (title,))[0][0]

        for genre in genre_list:
            row = execute_read_query("SELECT * FROM Genres WHERE GenreName = ?", (genre,))
            if len(row) == 0:
                execute_query("INSERT INTO Genres (GenreName, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy) VALUES (?, ?, ?, ?, ?)", (genre, created_at, created_by, modified_at, modified_by))
            genre_id = execute_read_query("SELECT GenreID FROM Genres WHERE GenreName = ?", (genre,))[0][0]
            if len(execute_read_query("SELECT * FROM BookGenres WHERE BookID = ? AND GenreID = ?", (book_id, genre_id,))) == 0:
                execute_query("INSERT INTO BookGenres (BookID, GenreID, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy) VALUES (?, ?, ?, ?, ?, ?)", (book_id, genre_id, created_at, created_by, modified_at, modified_by))
        
        #get chapter list
        r_chapter = session.get(link + '/muc-luc?page=all')
        chapter_links = r_chapter.html.find('.chuong-link') 
        for chapter_link in chapter_links:
            print(title + ' chuong ' + str(chapter_links.index(chapter_link) + 1) )
            get_chapter_info(BASE + chapter_link.attrs['href'], book_id, chapter_links.index(chapter_link) + 1)
    except:
        print('error')
        return

for i in range (0,12):
    result = session.get('https://bachngocsach.com.vn/reader/recent-bns?page=' + str(i))
    book_links = result.html.find('.recent-anhbia-a')
    for book_link in book_links:
        print(book_link.attrs['href'])
        get_book_info(BASE + book_link.attrs['href'])


