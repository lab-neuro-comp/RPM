import os
import re
import sys

def rename(inlet, pattern):
    match = pattern.search(inlet).group(0)
    index = int(match[1:-1])
    outlet = inlet.replace(match, '.%d.' % (index))
    os.system('move %s %s' % (inlet, outlet))

if __name__ == '__main__':
    pattern = re.compile(r'_[0-9].')
    for it in os.listdir('.'):
        rename(it, pattern)
