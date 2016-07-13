import os
import re

def valid_name(inlet):
    return True if re.search('\.png', inlet) else False

def resize(inlet):
    os.system('convert {0} -resize 130x100 {0}'.format(inlet))

if __name__ == '__main__':
    for it in filter(valid_name, os.listdir('.')):
        resize(it)
