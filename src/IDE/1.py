import sys
i = sys.argv[-1]

for x in open(i):
    print(x.rstrip())
