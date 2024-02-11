from PyQt5.QtWidgets import QApplication, QMainWindow, QFileDialog, QMessageBox
from PyQt5.QtGui import QPainter, QPen, QImage, QIcon
from PyQt5.QtCore import Qt, QTimer
from PyQt5 import uic

from subprocess import *

import sys

import os

ptm = os.path.abspath(os.getcwd()) + "\\GMIMachine.exe"

class Window(QMainWindow):
    def __init__(self):
        """
        класс окна с роботом
        """
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
        """
        paintEvent отрисовывает координатную плоскость и перемещения робота
        """
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
        """
        te метод с которым соединен таймер,
         вызывается каждый раз при срабатывании таймера, меняет координаты робота
        :return:
        """
        if self.i >= len(self.pts):
            self.t.stop()
            return
        if self.pts[self.i][0] == 'X':
            if self.x < self.pts[self.i][1]:
                self.x += 1
            elif self.x > self.pts[self.i][1]:
                self.x -= 1
            else:
                self.i += 1
        elif self.pts[self.i][0] == 'Y':
            if self.y < self.pts[self.i][1]:
                self.y += 1
            elif self.y > self.pts[self.i][1]:
                self.y -= 1
            else:
                self.i += 1
        self.update()

    def somerror(self):
        """
        somerror метод, который срабатывает при получении исключения от интерпретатора,
        создает всплывающее окно с информацией об ошибке
        """
        poperr = QMessageBox()
        poperr.setWindowTitle("exception raised")
        poperr.setText(str(self.errs))
        poperr.setStandardButtons(QMessageBox.Ok | QMessageBox.Cancel)
        poperr.exec_()

    def run(self):
        """
        run срабатывает при запуске робота, отправляет путь к текстовому файлу интерпретатору и
        получает координаты или исключение.
        если есть готовые координаты, то запускает робота по ним, без обращения к интерпретатору.
        на каждый запуск по новым координатам записывает изменения в crashlog при исключении или
        в log, если исключение не срабатывало
        """
        if not self.pts:
            si = STARTUPINFO()
            si.dwFlags | STARTF_USESHOWWINDOW
            si.wShowWindow = SW_HIDE
            f = Popen([ptm, T.file],
                      stdin=PIPE, stdout=PIPE, stderr=PIPE, encoding="CP866", startupinfo=si,
                      universal_newlines=True, creationflags=CREATE_NO_WINDOW)
            cords, errs = f.stdout.read(), f.stderr.readline()
            cords = cords.rstrip()
            self.errs = errs
            f.kill()
            del f
            if self.errs:
                with open(T.file[:-4] + "crashlog.log", 'a') as crash:
                    crash.write('_' * 50 + '\n')
                    crash.write(self.errs + '\n')
                self.somerror()
                return
            if not cords:
                self.show()
                return
            with open(T.file[:-4] + "log.log", "a") as log:
                log.write('_' * 50 + '\n')
                for c in cords.split('\n'):
                    log.write(c + '\n')
                    n = c.split(" >> ")
                    self.pts.append([n[0], int(n[1])])
            self.show()
            self.t.start(100)
        else:
            self.i = 0
            self.x = 0
            self.y = 0
            self.show()
            self.t.start(100)

    def stop(self):
        """
        stop останавливает таймер, что останавливавет метод te
        """
        self.t.stop()

    def cont(self):
        """
        cont возобнавляет таймер, если робот не достиг своей финальной точки, что запускает метод te
        """
        if not self.i >= len(self.pts):
            self.t.start(100)


class Textui(QMainWindow):
    def __init__(self):
        """
        класс окна с текстовым редактором и меню с командами для робота
        """
        QMainWindow.__init__(self)
        uic.loadUi("textedit.ui", self)
        self.fl = True
        self.file = ""

        self.setWindowIcon(QIcon("icon.png"))
        self.actionRun.triggered.connect(self.run)
        self.actionStop.triggered.connect(w.stop)
        self.actionSave.triggered.connect(self.save)
        self.actionSave_as.triggered.connect(self.saveas)
        self.actionOpen.triggered.connect(self.open)
        self.actionContinue.triggered.connect(w.cont)
        self.textedit.textChanged.connect(self.change)
        self.show()

    def change(self):
        """
        change срабатывает при изменении текста и меняет флаг на True, что нужно для проверки перед запуском робота
        """
        self.fl = True

    def run(self):
        """
        run проверяет текст на изменения, если изменений не было, то запускает робота, если были, то
        предлагает сохранить файл
        """
        if not self.fl:
            w.run()
        else:
            wanna = QMessageBox()
            wanna.setText("Ваш код должен быть сохранен перед запуском. \n"
                          "\t\tOk чтобы сохранить")
            wanna.setStandardButtons(QMessageBox.Ok | QMessageBox.Cancel)
            ret = wanna.exec()
            if ret == QMessageBox.Ok:
                self.save()

    def save(self):
        """
        save сохраняет файл в нынешний или запускает saveas, если файл еще не выбран
        """
        if self.file:
            with open(self.file, "w", encoding='utf-8') as of:
                of.write(self.textedit.toPlainText())
        else:
            self.saveas()
        self.fl = False
        w.pts = []
        w.errs = []
        w.i = 0
        w.x = 0
        w.y = 0

    def open(self):
        """
        open открывает диалог выбора файла и загружает выбранный файл в окно текстового редактора
        """
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
                with open(self.file, 'r', encoding="utf-8") as f:
                    t = f.read()
                    self.textedit.setText(t)
                    self.save()
                    self.fl = False
            except FileNotFoundError:
                pop = QMessageBox()
                pop.setText("Не удалось найти этот файл")
                pop.setWindowTitle("Ошибка")
                pop.setStandardButtons(QMessageBox.Ok | QMessageBox.Cancel)
                pop.exec_()

    def saveas(self):
        fsd = QFileDialog()
        """
        saveas открывает окно для выбора файла, в который будет записана нынешняя программа
        """
        fsd.setFileMode(QFileDialog.AnyFile)
        fsd.setWindowTitle("save as")
        fsd.setNameFilter("text files (*.txt)")
        fsd.setViewMode(QFileDialog.Detail)
        if fsd.exec_():
            self.file = fsd.selectedFiles()[-1]
            if self.file[-4:] != ".txt":
                self.file += ".txt"
            self.save()


app = QApplication(sys.argv)

w = Window()
T = Textui()

sys.exit(app.exec_())
