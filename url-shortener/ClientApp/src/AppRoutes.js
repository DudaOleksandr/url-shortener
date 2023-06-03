import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";
import { Login } from "./components/Auth/Login";
import { Register } from "./components/Auth/Register";
import {ShortUrls} from "./components/ShortUrl/ShortUrl";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
    path: '/login',
    element: <Login />
  },
  {
    path: '/register',
    element: <Register />
  },
  {
    path: '/fetch-data',
    element: <FetchData />
  },
  {
    path: '/short-urls',
    element: <ShortUrls />
  }
];

export default AppRoutes;
