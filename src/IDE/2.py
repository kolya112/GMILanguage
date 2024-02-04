from PyQt5.QtWidgets import QApplication, QMainWindow, QPushButton, QFileDialog, QTextEdit, QMessageBox
from PyQt5.QtGui import QPainter, QPen, QImage, QIcon
from PyQt5.QtCore import Qt, QPointF, QTimer
from PyQt5 import uic

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
        self.setWindowIcon(QIcon("icon.png"))
        self.setGeometry(700, 300, 630, 630)
        self.setWindowTitle("Surface")
        self.setMaximumSize(630, 630)
        self.setMinimumSize(630, 630)
        self.file = ""
        self.x = 0
        self.y = 0
        self.v = 30
    def paintEvent(self, pe):
        painter = QPainter(self)
        x = 0
        painter.setPen(QPen(Qt.lightGray, 1, Qt.SolidLine))
        while x <= self.width():
            painter.drawLine(x, 0, x, self.height())
            x += self.v
        y = 0
        while y <= self.height():
            painter.drawLine(0, y, self.width(), y)
            y += self.v
        painter.drawImage(self.x * self.v, self.v * 20 - self.y * self.v,
                          self.img)
    def te(self):
        if self.i >= len(self.pts):
            self.t.stop()
            return
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
    def run(self):
        self.show()
        if not self.pts:
            f = Popen(["python", "1.py"], stdin=PIPE, stdout=PIPE, stderr=PIPE, encoding="utf-8")
            cords, errs = f.communicate(input=r"C:\Users\anton\Desktop\frbk\sp\GMILanguage\src\IDE\файл.txt")
            cords = [tuple(map(int, x.rstrip().split())) for x in
                     cords.rstrip().split("\n")]
            self.pts = cords
            self.errs = errs
        self.t.start(100)
    def stop(self):
        self.t.stop()

class Textui(QMainWindow):
    def __init__(self):
        QMainWindow.__init__(self)
        uic.loadUi("textedit.ui", self)
        self.fl = True
        self.file = ""

        self.setWindowIcon(QIcon("icon.png"))
        self.actionRun.triggered.connect(self.run)
        self.actionStop.triggered.connect(self.stop)
        self.actionSave.triggered.connect(self.save)
        self.actionSave_as.triggered.connect(self.saveas)
        self.actionOpen.triggered.connect(self.open)
        self.textedit.textChanged.connect(self.change)
        self.show()
    def change(self):
        self.fl = True
    def run(self):
        if self.fl == False:
            w.run()
        else:
            wanna = QMessageBox()
            wanna.setText("Ваш код должен быть сохранен перед запуском. \n"
                          "\t\tOk чтобы сохранить")
            wanna.setStandardButtons(QMessageBox.Ok | QMessageBox.Cancel)
            wanna
            ret = wanna.exec()
            if ret == QMessageBox.Ok:
                self.save()
    def stop(self):
        w.stop()
    def cont(self):
        pass
    def save(self):
        if self.file:
            with open(self.file, "w") as of:
                of.write(self.textedit.toPlainText())
        else:
            self.saveas()
        self.fl = False
    def open(self):
        fod = QFileDialog()
        fod.setFileMode(QFileDialog.AnyFile)
        fod.setWindowTitle("open")
        fod.setNameFilter("text files (*.txt)")
        fod.setViewMode(QFileDialog.Detail)
        if fod.exec_():
            self.file = fod.selectedFiles()[-1]
            if self.file[-4:] != ".txt":
                self.file += ".txt"
            try:
                with open(self.file, 'r') as f:
                    t = f.read()
                    self.textedit.setText(t)
            except FileNotFoundError:
                pop = QMessageBox()
                pop.setText("Не удалось найти этот файл")
                pop.setWindowTitle("Ошибка")
                pop.setStandardButtons(QMessageBox.Ok)
                pop.exec_()
    def saveas(self):
        fsd = QFileDialog()
        fsd.setFileMode(QFileDialog.AnyFile)
        fsd.setWindowTitle("save as")
        fsd.setNameFilter("text files (*.txt)")
        fsd.setViewMode(QFileDialog.Detail)
        if fsd.exec_():
            self.file = fsd.selectedFiles()[-1]
            if self.file[-4:] != ".txt": self.file += ".txt"
            self.save()

app = QApplication(sys.argv)

w = Window()
T = Textui()

sys.exit(app.exec_())
