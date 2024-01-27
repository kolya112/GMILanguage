from PyQt5 import QtGui
from PyQt5.QtWidgets import QApplication, QMainWindow, QWidget, QPushButton
from PyQt5.QtGui import QPainter, QBrush, QPen, QPolygonF, QPainterPath, QImage
from PyQt5.QtCore import Qt, QPointF, QTimer

from subprocess import *

import sys

class Window(QMainWindow):
    def __init__(self):
        QMainWindow.__init__(self)
        self.pts = []
        self.i = 0
        self.t = QTimer(self)
        self.t.timeout.connect(self.te)
        self.img = QImage("robot.png")
        self.x = 0
        self.y = 0
        self.vx = 0
        self.vy = 0
        self.v = 15
    def paintEvent(self, pe):
        painter = QPainter(self)
        painter.translate(self.width() // 2, self.height() // 2)
        x = 0
        painter.setPen(QPen(Qt.gray, 4, Qt.SolidLine))
        while x < self.width() // 2:
            painter.drawLine(x, -self.height() // 2, x, self.height() // 2)
            painter.drawLine(-x, -self.height() // 2, -x, self.height() // 2)
            x += 50
            painter.setPen(QPen(Qt.gray, 1, Qt.SolidLine))
        y = 0
        painter.setPen(QPen(Qt.gray, 4, Qt.SolidLine))
        while y < self.height() // 2:
            painter.drawLine(-self.width() // 2, y, self.width() // 2, y)
            painter.drawLine(-self.width() // 2, -y, self.width() // 2, -y)
            y += 50
            painter.setPen(QPen(Qt.gray, 1, Qt.SolidLine))
        painter.drawImage(int(self.x-self.img.width() // 2), int(self.y - self.img.height() // 2), self.img)
    def te(self):
        if self.atEnd():
            self.x = self.pts[self.i].x()
            self.y = self.pts[self.i].y()
            self.i += 1
            if self.i >= len(self.pts):
                self.t.stop()
                self.update()
                return
            self.chngVelo()
        self.x += self.vx
        self.y += self.vy
        self.update()
    def start(self):
        f = Popen(["python", "1.py"], stdin=PIPE, stdout=PIPE, stderr=PIPE, encoding="utf-8")
        cords = f.communicate(input=r"C:\Users\anton\Desktop\frbk\sp\файл.txt")[0]
        cords = [QPointF(tuple(map(float, x.rstrip().split()))[0], -tuple(map(float, x.rstrip().split()))[1]) for x in
                 cords.rstrip().split("\n")]
        self.pts = cords
        self.i = 0
        self.chngVelo()
        self.t.start(100)
    def atEnd(self):
        if self.currentDistance() <= self.v:
            return True
        else:
            return False
    def currentDistance(self):
        return Window.distance(self.pts[self.i].x() - self.x, self.pts[self.i].y() - self.y)
    def distance(x, y):
        return (x ** 2 + y ** 2) ** 0.5
    def edgeLength(x1, y1, x2, y2):
        return Window.distance(x2 - x1, y2 - y1)
    def chngVelo(self):
        self.vx = self.v / Window.edgeLength(self.x, self.y, self.pts[self.i].x(), self.pts[self.i].y()) * (self.pts[self.i].x() - self.x)
        self.vy = self.v / Window.edgeLength(self.x, self.y, self.pts[self.i].x(), self.pts[self.i].y()) * (self.pts[self.i].y() - self.y)




app = QApplication(sys.argv)

w = Window()
w.setGeometry(600, 300, 500, 500)
w.show()

bs = QPushButton(w)
bs.setGeometry(150, 0, 100, 30)
bs.setText("start")
bs.clicked.connect(w.start)
bs.show()

sys.exit(app.exec_())
