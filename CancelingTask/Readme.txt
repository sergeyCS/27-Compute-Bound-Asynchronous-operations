Задание - показать, разницу и похожее при отмене таски и при необработанном исключении в нём.
Запустить таску, в запущенную функцию передать токен, в саму таску передать тот же токен (это не обязательно, и без этого сработает).
Дать поработать или вызвать отмену,
потом обратиться к t.Result, и с помощью AggregateException.Handle обработать внутрение исключения.