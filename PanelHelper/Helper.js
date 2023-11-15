let ip = "";
let port = "16021";
let token = "";

let orientation = 0;
let panels = null;

let orientAdjust = 0;

async function onGetClicked() {
  ip = document.getElementById("ip").value;
  token = document.getElementById("token").value;
  const response = await fetch(`http://${ip}:${port}/api/v1/${token}`);
  const jsonData = await response.json();
  const layout = jsonData.panelLayout;
  orientation = layout.globalOrientation.value;
  panels = layout.layout.positionData;
  origPanels = JSON.parse(JSON.stringify(panels));
  drawPoints();
}

function drawPoints() {
  const canvas = document.getElementById("pointCanvas");
  const context = canvas.getContext("2d");

  let bigX = 0;
  let bigY = 0;
  let minX = 0;
  let minY = 0;
  for (const p of panels) {
    if (p.x > bigX) {
      bigX = p.x;
    }
    if (p.y > bigY) {
      bigY = p.y;
    }
  }
  const o = orientation + orientAdjust;
  panels = JSON.parse(JSON.stringify(origPanels));
  const s = Math.sin(o);
  const c = Math.cos(o);
  for (const p of panels) {
    const x = p.x * c - p.y * s;
    const y = p.x * s + p.y * c;
	p.x = x;
	p.y = y;
    if (p.x < minX) minX = p.x;
    if (p.y < minY) minY = p.y;
  }

  for (const p of panels) {
    p.x += Math.abs(minX);
    p.y += Math.abs(minY);
    p.x = Math.floor(p.x);
    p.y = Math.floor(p.y);
  }

  for (const p of panels) {
    if (p.x > bigX) {
      bigX = p.x;
    }
    if (p.y > bigY) {
      bigY = p.y;
    }
  }

  context.canvas.width = bigX;
  context.canvas.height = bigY;
  context.clearRect(0, 0, canvas.width, canvas.height);

  for (const p of panels) {
    context.fillRect(p.x, p.y, 3, 3);
  }
}

function onAdjustInput(value) {
  orientAdjust = +value;
  drawPoints();
}
