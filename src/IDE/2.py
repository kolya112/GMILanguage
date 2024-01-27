from PyQt5.QtWidgets import QApplication, QMainWindow, QPushButton
from PyQt5.QtGui import QPainter, QPen, QImage
from PyQt5.QtCore import Qt, QPointF, QTimer

from subprocess import *

import sys

class Window(QMainWindow):
    def __init__(self):
        QMainWindow.__init__(self)
        self.pts = []
        self.errs = []
        self.i = 0
        self.t = QTimer(self)
        self.t.timeout.connect(self.te)
        self.img = QImage("robot.png")
        self.setGeometry(500, 300, 630, 630)
        self.setMaximumSize(630, 630)
        self.setMinimumSize(630, 630)
        self.show()
        self.x = 0
        self.y = 0
        self.v = 30
    def paintEvent(self, pe):
        painter = QPainter(self)
        x = 0
        painter.setPen(QPen(Qt.gray, 1, Qt.SolidLine))
        while x <= self.width():
            painter.drawLine(x, 0, x, self.height())
            x += self.v
        y = 0
        while y <= self.height():
            painter.drawLine(0, y, self.width(), y)
            y += self.v
        painter.drawImage(self.x * self.v, self.v * 20 - self.y * self.v,
                          self.img,
                          sw=self.v, sh=self.v)
    def te(self):
        if self.x < self.pts[self.i][0]:
            self.x += 1
        elif self.x > self.pts[self.i][0]:
            self.x -= 1
        elif self.y < self.pts[self.i][1]:
            self.y += 1
        elif self.y > self.pts[self.i][1]:
            self.y -= 1
        else:
            self.i += 1
            if self.i >= len(self.pts):
                self.t.stop()
        self.update()
    def start(self):
        f = Popen(["python", "1.py"], stdin=PIPE, stdout=PIPE, stderr=PIPE, encoding="utf-8")
        cords, errs = f.communicate(input=r"C:\Users\anton\Desktop\frbk\sp\GMILanguage\src\IDE\файл.txt")
        cords = [tuple(map(int, x.rstrip().split())) for x in
                 cords.rstrip().split("\n")]
        self.pts = cords
        self.errs = errs
        self.i = 0
        self.t.start(100)



app = QApplication(sys.argv)

w = Window()

bs = QPushButton(w)
bs.setGeometry(150, 0, 100, 30)
bs.setText("start")
bs.clicked.connect(w.start)
bs.show()

sys.exit(app.exec_())
