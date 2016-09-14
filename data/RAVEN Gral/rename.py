import os
import re
import sys

def rename(inlet, pattern):
    outlet = pattern.sub('.', inlet)
    if inlet != outlet:
        os.system('move %s %s' % (inlet, outlet))

if __name__ == '__main__':
    
    pattern = re.compile('-')
    for it in os.listdir('.'):
        rename(it, pattern)
